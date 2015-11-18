using IServices;
using Models;
using Models.QueryModel;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SystemSet.Controllers
{
    public class AllianceNameListController : BaseController
    {
        private IAllianceNameListService _IAllianceNameList;
        private IUser _Iuser;
        private int pagesize = 20;
        public AllianceNameListController(IAllianceNameListService ianls, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IAllianceNameList = ianls;
            _Iuser = employeeService.User;
        }
        public ActionResult Index()
        {
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "足球聯盟名稱對應表" },
                HaveButton = false
            };
            return View(new List<AllianceNameList>());
        }

        public ActionResult AddOrQuery(int ddlItem, string ddlGameType, string ddlLanguagecode, string FullName, string SimpleName, int pageIndex = 1)
        {
            ViewBag.isAddCG = string.Empty;
            if (ddlItem == 1)
            {
                AllianceNameList anl = new AllianceNameList()
                {
                    AllianceType = ddlGameType,
                    SimpleName = SimpleName,
                    FullName = FullName,
                    LanguageCode = ddlLanguagecode,
                    Creator = _Iuser.UserName,
                    CreateTime = DateTime.Now
                };
                _IAllianceNameList.Add(anl);
                if (_IAllianceNameList.Commit() > 0)
                {
                    ViewBag.isAddCG = "success";
                }
                ViewBag.navigation = new Navigation
                {
                    Level = new List<string> { "系統設定", "足球聯盟名稱對應表" },
                    HaveButton = false
                };
                return View("Index", new List<Models.AllianceNameList>());
            }
            else
            {
                bool FullNameIsNull = string.IsNullOrWhiteSpace(FullName);
                bool SimpleNameIsNull = string.IsNullOrWhiteSpace(SimpleName);
                int count = 0;
                IQueryable<AllianceNameList> qu = _IAllianceNameList.QueryByConditionForPage((p => p.AllianceType == ddlGameType && p.LanguageCode == ddlLanguagecode && (FullNameIsNull ? true : p.FullName.Contains(FullName)) && (SimpleNameIsNull ? true : p.SimpleName.Contains(SimpleName))), p => p.GUID, pageIndex, pagesize, out count);
                AllianceNameListQuery anQ = new AllianceNameListQuery() { ddlItem = ddlItem, ddlGameType = ddlGameType, ddlLanguagecode = ddlLanguagecode, FullName = FullName, SimpleName = SimpleName };
                ViewBag.pageIndex = pageIndex;
                ViewBag.pageSize = pagesize;
                ViewBag.recordCount = count;
                ViewBag.queryModel = anQ;
                ViewBag.navigation = new Navigation
                {
                    Level = new List<string> { "系統設定", "足球聯盟名稱對應表" },
                    HaveButton = false
                };
                return View("Index", qu);
            }
        }

        [HttpPost]
        public ActionResult Edit(AllianceNameList anl)
        {
            AllianceNameList old = _IAllianceNameList.QueryById(anl.GUID);
            if (old != null && !string.IsNullOrWhiteSpace(anl.FullName) && !string.IsNullOrWhiteSpace(anl.SimpleName))
            {
                old.SimpleName = anl.SimpleName;
                old.FullName = anl.FullName;
                old.Modifier = _Iuser.UserName;
                old.ModifyTime = DateTime.Now;
                _IAllianceNameList.Update(old);
                if (_IAllianceNameList.Commit() > 0)
                {
                    return Json("success");
                }
            }
            return Json("error");
        }
        [HttpPost]
        public ActionResult Delete(int GUID)
        {

            AllianceNameList old = _IAllianceNameList.QueryById(GUID);
            if (old != null)
            {
                _IAllianceNameList.Delete(old);
                if (_IAllianceNameList.Commit() > 0)
                {
                    return Json("success");
                }
            }
            return Json("error");
        }

    }
}
