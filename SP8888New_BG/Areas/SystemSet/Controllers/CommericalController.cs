using Common;
using IServices;
using Models;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SystemSet.Controllers
{
    public class CommericalController : BaseController
    {
        //
        // GET: /SystemSet/Commerical/

        private ICommericalService _ICommericalService;
        public CommericalController(ICommericalService commericalService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _ICommericalService = commericalService;
        }
        public ActionResult Index(string language)
        {
            Dictionary<string, Tuple<int, List<CommercialImage>>> commercial = _ICommericalService.GetCommercial(language);
            ViewBag.Language = language;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", language == "tw" ? "繁體 PC 版廣告管理" : "簡體 PC 版廣告管理" },
                HaveButton = false
            };
            return View(commercial);
        }
        public ActionResult UpDateCommercial(CommercialImage image, HttpPostedFileBase commercial, string act, string oldImageName, string language, string direction)
        {
            if (commercial != null)
            {
                image.ImageName = commercial.FileName;
            }
            int n = _ICommericalService.EditCommercial(image, act, commercial, oldImageName, language, direction);
            return Json(n);
        }
        public ActionResult ChangeSeconds(int seconds, string direction, string language)
        {
            int n = _ICommericalService.ChangeCommercialSeconds(seconds, direction, language);
            return Json(n);
        }
    }
}
