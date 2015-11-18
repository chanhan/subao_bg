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
    public class NameControlController :BaseController
    {
        //
        // GET: /SystemSet/NameControl/
        private INameControlService _INameControlService;
        public NameControlController(INameControlService nameControlService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _INameControlService = nameControlService;
        }
        public ActionResult Index(NameControl nameControl)
        {
            if (string.IsNullOrEmpty(nameControl.Category))
            {
                nameControl = new NameControl
                {
                    AppType = "First",
                    GTLangx = "TN",
                    GameType = "Name",
                    Category = "2",
                    Langx = "en"
                };
            }
            List<NameControl> nameControls = _INameControlService.GetNameControls(nameControl);
            ViewBag.nameControl = nameControl;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "名稱對應表" },
                HaveButton = false
            };
            return View(nameControls);
        }
        [HttpPost]
        public ActionResult EditNameControl(NameControl nameControl)
        {
            int n = _INameControlService.EditNameControl(nameControl);
            return Json(n);
        }
        [HttpPost]
        public ActionResult DeleteNameControl(int id)
        {
           int n = _INameControlService.DeleteNameControl(id);
            return Json(n);
        }
    }
}
