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

namespace SP8888New_BG.Areas.IceHockey.Controllers
{
    public class IHBFAllianceController : BaseController
    {
        private IIceHockeyAllianceService _IIceHockeyAllianceService;

        public IHBFAllianceController(IEmployeeService employeeService, IIceHockeyAllianceService IIceHockeyAlliance)
            : base(employeeService)
        {
            _IIceHockeyAllianceService = IIceHockeyAlliance;
        }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        private int pageSize = 15;

        /// <summary>
        /// 主頁顯示
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string gameType = "IHBF", string keyWords = null, int pageIndex = 1, string sMsg = null)
        {
            int count = 0;
            List<IceHockeyAlliance> ia = _IIceHockeyAllianceService.getAllianceListByIHBF(gameType, keyWords, pageIndex, pageSize, out count);
            PagerInfo pager = new PagerInfo(pageIndex, pageSize, count);
            PagerQuery<PagerInfo, List<IceHockeyAlliance>, string> query = new PagerQuery<PagerInfo, List<IceHockeyAlliance>, string>(pager, ia, keyWords);
            ViewBag.PageCount = count;
            ViewBag.gameType = gameType.ToUpper();
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "聯盟管理" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = "IHBFSchedules",
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", gameType)
                },
                ButtonText = "返回賽程"
            };
            return View(query);
        }

        /// <summary>
        /// 修改请求
        /// </summary>
        [HttpPost]
        public ActionResult Edit(IceHockeyAlliance ia, string keyWord = null, int pageIndex = 1)
        {
            return Json(_IIceHockeyAllianceService.EditAlliance(ia));
        }

        /// <summary>
        /// 保存显示设定
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveSetting(List<IceHockeyAlliance> list)
        {
            int c = _IIceHockeyAllianceService.SaveDisplaySetting(list);
            return Json(new { count = c });
        }
    }
}
