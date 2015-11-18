using Common;
using IServices;
using Models;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SystemSet.Controllers
{
    public class MarqueeController : BaseController
    {
        //
        // GET: /SystemSet/Marquee/
        private IMarqueeService _IMarqueeService;
        public MarqueeController(IMarqueeService marqueeService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IMarqueeService = marqueeService;
        }
        public ActionResult Index(string gameType,bool? isAdd)
        {
            if (isAdd == null) isAdd = true;
            gameType = gameType.ToUpper();
            bool all = gameType == "AAAA";
            List<Marquee> marquees = _IMarqueeService.QueryByCondition(p => all ? true : p.GameType == gameType).OrderByDescending(p => p.ChgTime).ToList().OrderBy(p => p.EnableYN, new MarqueeCompare()).ToList();
            int year = DateTime.Now.Year;
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = year.ToString(), Value = year.ToString() });
            list.Add(new SelectListItem { Text = (year + 1).ToString(), Value = (year + 1).ToString().ToString() });
            list.Add(new SelectListItem { Text = (year + 2).ToString().ToString(), Value = (year + 2).ToString().ToString() });
            ViewBag.year = list;
            ViewBag.gameType = gameType;
            ViewBag.isAdd = isAdd;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "訊息管理" },
                HaveButton = false
            };
            return View(marquees);
        }
        public ActionResult DeleteMarquee(int id)
        {
            int n = _IMarqueeService.DeleteMarquee(id);
            return Json(n);
        }
        public ActionResult UpDateMarquee(Marquee marquee)
        {
            int n = _IMarqueeService.UpDateMarquee(marquee);
            return RedirectToAction("Index", new { gameType = "AAAA" });
        }
        [HttpPost]
        public ActionResult SetEnableMarquee(int id, string enableYN)
        {
            int n = _IMarqueeService.autoClose(id, enableYN);
            return Json(n);
        }
    }
}
