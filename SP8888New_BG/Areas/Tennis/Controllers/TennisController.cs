using Common;
using Helper.Pager;
using IServices;
using Models;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.Tennis.Controllers
{
    public class TennisController : BaseController
    {
        //
        // GET: /Tennis/Tennis/
        private ITennisService _ITennisService;
        private ITennisAllianceService _ITennisAllianceService;
        public TennisController(ITennisService tennisService, ITennisAllianceService tennisAllianceService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _ITennisService = tennisService;
            _ITennisAllianceService = tennisAllianceService;
        }
        public ActionResult Index(DateTime date)
        {
            List<Models.ViewModel.Tennis> schedules = _ITennisService.GetTennisSchedules(date);
            List<TennisAlliance> alliances = schedules.Select(p => new TennisAlliance { AllianceID = p.AllianceID, AllianceName = p.AllianceName, ShowName = p.AllianceShowName, Display = p.AllianceDisPlay }).Distinct(new TennisAllianceComparer()).ToList();
            ViewBag.Alliances = alliances;
            ViewBag.SelectAlliances = alliances.Select(c => new SelectListItem { Text = c.ShowName, Value = c.AllianceID.ToString() });
            ViewBag.date = date.ToString("yyyy-MM-dd");
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "網球", "賽程資料" },
                HaveButton = false
            };
            return View(schedules);
        }

        [HttpPost]
        public ActionResult Index(IList<TennisSchedules> schedules)
        {
            int n = _ITennisService.SaveSchedules(schedules);
            return Json(new { count = n });
        }

        public ActionResult TennisAlliance(string keyWords, int pageIndex = 1, int pageSize = 20)
        {
            int count = 0;
            List<TennisAlliance> pagedList = _ITennisAllianceService.GetTennisAllianceByCondition(keyWords, pageIndex, pageSize, out count);
            PagerInfo pager = new PagerInfo(pageIndex, pageSize, count);
            PagerQuery<PagerInfo, List<TennisAlliance>, string> query = new PagerQuery<PagerInfo, List<TennisAlliance>, string>(pager, pagedList, keyWords);
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "網球", "聯盟資料" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("date",DateTime.Now.ToString("yyyy-MM-dd")),
                }
            };
            return View(query);
        }

        [HttpPost]
        public ActionResult TennisAlliance(IList<TennisAlliance> alliance)
        {
            int n = _ITennisAllianceService.AllianceDispalySet(alliance);
            return Json(new { count = n });
        }
        [HttpPost]
        public ActionResult UpdateAlliance(TennisAlliance alliance)
        {
            TennisAlliance checkAlliance = _ITennisAllianceService.QueryByCondition(p => p.ShowName == alliance.ShowName && p.AllianceID != alliance.AllianceID).SingleOrDefault();
            if (checkAlliance!=null) return Json(-1);
            _ITennisAllianceService.Update(alliance);
            int n = _ITennisAllianceService.Commit();
            return Json(n);
        }
    }
}
