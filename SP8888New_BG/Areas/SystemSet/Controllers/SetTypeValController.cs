using IServices;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SystemSet.Controllers
{
    public class SetTypeValController : BaseController
    {
        private ISetTypeValService _ISetTypeValService;
        public SetTypeValController(ISetTypeValService settypevalservice, IEmployeeService emp)
            : base(emp)
        {
            _ISetTypeValService = settypevalservice;
        }
        //
        // GET: /SetTypeVal/SetTypeVal/

        public ActionResult Index()
        {
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "首頁設定" },
                HaveButton = false
            };
            var val = _ISetTypeValService.GetDataSetTypeVal().Split(',');
            if (val.Length > 1)
            {
                ViewBag.tuple = Tuple.Create(val[0], val[1]);
            }
            return View();
        }


        public ActionResult EditSetTypeVal(string Val1, string Val2)
        {
            var edit = _ISetTypeValService.EditSetTypeVal(Val1, Val2);
            return Json(edit);
        }
    }
}
