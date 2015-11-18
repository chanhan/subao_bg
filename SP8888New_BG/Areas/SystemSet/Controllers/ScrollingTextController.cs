using IServices;
using Models;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SystemSet.Controllers
{
    public class ScrollingTextController : BaseController
    {
        //
        // GET: /SystemSet/ScrollingText/
        private IScrollingTextService _IScrollingTextService;
        public ScrollingTextController(IScrollingTextService scrollingTextService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IScrollingTextService = scrollingTextService;
        }
        public ActionResult Index(ScrollingText scrolling)
        {
            List<ScrollingText> scrollings = _IScrollingTextService.QueryByCondition(p => (string.IsNullOrEmpty(scrolling.LanguageCode) ? true : p.LanguageCode == scrolling.LanguageCode) && (scrolling.Visible == null ? true : p.Visible == scrolling.Visible) && ((scrolling.StartTime != null && scrolling.EndTime != null) ? ((p.StartTime.Value.CompareTo(scrolling.StartTime.Value) >= 0 && p.StartTime.Value.CompareTo(scrolling.EndTime.Value) <= 0) || (p.EndTime.Value.CompareTo(scrolling.StartTime.Value) >= 0 && p.EndTime.Value.CompareTo(scrolling.EndTime.Value) <= 0) || (scrolling.StartTime.Value.CompareTo(p.StartTime.Value) >= 0 && scrolling.StartTime.Value.CompareTo(p.EndTime.Value) <= 0)) : true)).ToList();
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "跑馬燈管理" },
                HaveButton = false
            };
            return View(scrollings);
        }
        public ActionResult EditScrollingText(ScrollingText scrolling)
        {
            int n = _IScrollingTextService.EditScrollingText(scrolling);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteScrollingText(int scrollingTextID)
        {
            _IScrollingTextService.Delete(scrollingTextID);
            int n = _IScrollingTextService.Commit();
            return Json(n);
        }
    }
}
