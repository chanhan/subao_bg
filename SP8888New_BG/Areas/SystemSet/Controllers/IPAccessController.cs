using Common;
using Helper.Pager;
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
    public class IPAccessController : BaseController
    {
        private IIPAccessService _IIPAccessService;
        private IUser _IUser;

        public IPAccessController(IIPAccessService ipaccessservice,IUser user,IEmployeeService emp):base(emp)
        {
            _IIPAccessService = ipaccessservice;
            _IUser = user;
        }
        //
        // GET: /IPAccess/IPAccess/

        public ActionResult Index(int pageIndex = 1, int pageSize = 20)
        {
            int count = 0;
            List<Models.IPAccess> ipaccess = _IIPAccessService.GetIPAccessAll(pageIndex, pageSize, out count);
            PagerInfo pager = new PagerInfo(pageIndex, pageSize, count);
            PagerQuery<PagerInfo, List<Models.IPAccess>, string> query = new PagerQuery<PagerInfo, List<Models.IPAccess>, string>(pager, ipaccess, "");
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "系統設定", "登入IP管理" },
                HaveButton = false
            };
            return View(query);
        }
        [HttpPost]
        public ActionResult Delete(int ID)
        {
            int del = _IIPAccessService.DeleteIPAccessById(ID);//删除
            return Json(del);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">id</param>
        /// <param name="IP">ip地址</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        public ActionResult Edit(int ID, string IP, string Remark)
        {
            int edit = _IIPAccessService.UpdateIPAccessById(ID, IP, Remark, _IUser.UserName);
            return Json(edit);
        }

    }
}
