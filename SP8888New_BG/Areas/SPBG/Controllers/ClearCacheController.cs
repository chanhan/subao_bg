using Common;
using IServices;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SPBG.Controllers
{
    public class ClearCacheController : BaseController
    {
        //
        // GET: /SPBG/Clear/
        public ClearCacheController(IEmployeeService employeeService)
            : base(employeeService)
        {
        }
        public ActionResult Index(string act)
        {
            bool isClear = false;
            if (!string.IsNullOrEmpty(act))
            {
                ClearAction.Clear(act);
                isClear = true;
            }
            ViewBag.isClear = isClear;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "清除緩存" },
                HaveButton = false
            };
            return View();
        }

    }
}
