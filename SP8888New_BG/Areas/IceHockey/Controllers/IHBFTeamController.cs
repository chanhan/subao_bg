using Common;
using Helper.Pager;
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

namespace SP8888New_BG.Areas.IceHockey.Controllers
{
    public class IHBFTeamController : BaseController
    {
        private IIceHockeyTeamService _IIceHockeyTeamService;
        private IIceHockeyAllianceService _IIceHockeyAllianceService;

        public IHBFTeamController(IEmployeeService employeeService, IIceHockeyTeamService its, IIceHockeyAllianceService ias)
            : base(employeeService)
        {
            _IIceHockeyTeamService = its;
            _IIceHockeyAllianceService = ias;
        }
        /// <summary>
        /// 每页显示数量
        /// </summary>
        private int pageSize = 15;

        /// <summary>
        /// 主页显示
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="keyWord"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(BKOSTeamQuery queryModel, string gameType = "IHBF", int pageIndex = 1, string sMsg = null)
        {
            //总数量
            int count = 0;

            //聯盟下拉框
            var alliance = _IIceHockeyAllianceService.QueryByCondition(p => p.GameType == gameType).ToList();
            List<SelectListItem> items = alliance.Select(p => new SelectListItem { Text = p.ShowName, Value = p.AllianceID.ToString(), Selected = queryModel.AllianceID == p.AllianceID }).ToList();
            ViewBag.alliance = items;

            //冰球BF和奥逊公用 一个视图模型 BKOSTeam
            List<BKOSTeam> list = _IIceHockeyTeamService.getTeamListByIHBF(gameType, queryModel, pageIndex, pageSize, out count);
            PagerInfo pager = new PagerInfo(pageIndex, pageSize, count);
            PagerQuery<PagerInfo, List<BKOSTeam>, BKOSTeamQuery> query = new PagerQuery<PagerInfo, List<BKOSTeam>, BKOSTeamQuery>(pager, list, queryModel);
            ViewBag.gameType = gameType.ToUpper();

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "隊伍管理" },
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
        public ActionResult Edit(IceHockeyTeam it, string keyWord = null, int pageIndex = 1)
        {
            return Json(_IIceHockeyTeamService.EditTeam(it));
        }
    }
}
