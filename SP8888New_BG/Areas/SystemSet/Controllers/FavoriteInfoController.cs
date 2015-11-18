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
    public class FavoriteInfoController : BaseController
    {
        private IFavoriteInfoService _IFavoriteInfoServce;
        private IUser _Iuser;
        public FavoriteInfoController(IFavoriteInfoService Ifi, IEmployeeService employeeService)
        : base(employeeService)
        {
            _IFavoriteInfoServce = Ifi;
            _Iuser = employeeService.User;
        }

        public ActionResult Index()
        {
            List<FavoriteInfo> list = _IFavoriteInfoServce.QueryAll().ToList();
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "收藏夾設定" },
                HaveButton = false
            };
            ViewBag.isEdit = false;
            return View(list);
        }
        public ActionResult Edit(int id)
        {
            List<FavoriteInfo> list = new List<FavoriteInfo>();
            FavoriteInfo fi = _IFavoriteInfoServce.QueryById(id);
            if (fi == null)
            {
                return RedirectToAction("Index");
            }
            list.Add(fi);
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "收藏夾設定修改" },
                Area = "SystemSet",
                Controller = "FavoriteInfo",
                Action = "Index",
                Parameter = new List<Models.ViewModel.Parameter> { new Parameter("gameType", "cs") },
                HaveButton = true,
                ButtonText = "返回收藏夾設定"
            };
            ViewBag.isEdit = true;
            return View("Index", list);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            FavoriteInfo fi = _IFavoriteInfoServce.QueryById(id);
            if (fi != null)
            {
                _IFavoriteInfoServce.Delete(fi);
                return Json(_IFavoriteInfoServce.Commit());
            }
            return Json(0);
        }

        [HttpPost]
        public ActionResult Edit(FavoriteInfo fi)
        {
            FavoriteInfo old = _IFavoriteInfoServce.QueryById(fi.FavoriteID);
            if (fi != null && checkModel(fi))
            {
                old.JumpUrl = fi.JumpUrl;
                old.SimplifiedDisplay = fi.SimplifiedDisplay;
                old.SimplifiedPrompt = fi.SimplifiedPrompt;
                old.TraditionalDisplay = fi.TraditionalDisplay;
                old.TraditionalPrompt = fi.TraditionalPrompt;
                _IFavoriteInfoServce.Update(old);
                return Json(_IFavoriteInfoServce.Commit());
            }
            return Json(0);
        }
        [HttpPost]
        public ActionResult Add(FavoriteInfo fi)
        {
            if (fi != null && checkModel(fi))
            {
                _IFavoriteInfoServce.Add(fi);
                return Json(_IFavoriteInfoServce.Commit());
            }
            return Json(0);
        }
        private bool checkModel(FavoriteInfo fi)
        {
            if (!string.IsNullOrWhiteSpace(fi.SimplifiedDisplay)
                 && !string.IsNullOrWhiteSpace(fi.SimplifiedPrompt)
                && !string.IsNullOrWhiteSpace(fi.TraditionalDisplay)
                && !string.IsNullOrWhiteSpace(fi.TraditionalPrompt)
                && !string.IsNullOrWhiteSpace(fi.JumpUrl))
            {
                return true;
            }
            return false;
        }
    }
}
