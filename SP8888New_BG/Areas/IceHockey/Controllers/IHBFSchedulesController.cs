using Common;
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
    /// <summary>
    /// 冰球BF控制器
    /// </summary>
    public class IHBFSchedulesController : BaseController
    {
        private IIceHockeySchedulesService _IIceHockeySchedulesService;
        private IIceHockeyAllianceService _IIceHockeyAllianceService;
        private IIceHockeyTeamService _IIceHockeyTeamService;
        private IScoreModifyRecordService _ISMRService;
        private string gameType;
        public IHBFSchedulesController(IEmployeeService employeeService, IIceHockeySchedulesService ihService, IIceHockeyAllianceService ia, IIceHockeyTeamService it, IScoreModifyRecordService ismrs)
            : base(employeeService)
        {
            _IIceHockeySchedulesService = ihService;
            _IIceHockeyAllianceService = ia;
            _IIceHockeyTeamService = it;
            _ISMRService = ismrs;
            gameType = "IHBF";
        }
        //赛程资料主页
        public ActionResult Index(DateTime date)
        {
            List<Models.ViewModel.IceHockey> ihbf = _IIceHockeySchedulesService.GetSchedules(date, gameType);

            //联盟下拉框
            List<Models.ViewModel.IceHockey> a = ihbf.Distinct(new Common.IHBFAllianceComparer()).ToList();
            List<SelectListItem> ddlAlliance = a.Select(p => new SelectListItem { Text = p.Alliance, Value = p.Alliance.Replace(" ", "").Replace(":", "").Trim() }).ToList();
            ViewBag.alliance = a;
            ViewBag.ddlAlliance = ddlAlliance;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "賽程資料" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = RouteData.Values["action"].ToString(),
                HaveButton = false
            };
            ViewBag.date = date.ToString("yyyy-MM-dd");
            ViewBag.gameType = gameType;
            return View(ihbf);
        }
        /// <summary>
        /// 赛程列表存储设定
        /// </summary>
        [HttpPost]
        public ActionResult SetItem(IEnumerable<Models.ViewModel.IceHockey> ihbf)
        {
            int c = _IIceHockeySchedulesService.SaveSetting(ihbf);

            return Json(new { count = c });
        }

        /// <summary>
        /// 修改分数主页
        /// </summary>
        public ActionResult EditScore(int GID)
        {
            IceHockeySchedules ih = _IIceHockeySchedulesService.QueryById(GID);
            ViewBag.TeamAName = _IIceHockeyTeamService.QueryById(ih.TeamAID).ShowName;
            ViewBag.TeamBName = _IIceHockeyTeamService.QueryById(ih.TeamBID).ShowName;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(ih.GameType), "修改分數" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", ih.GameDate.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", ih.GameType)
                },
                ButtonText = "返回賽程"
            };
            return View("ScoreEdit", ih);
        }

        /// <summary>
        /// 修改分数
        /// </summary>
        [HttpPost]
        public ActionResult EditScore(IceHockeySchedules ih, FormCollection collection)
        {
            string rA = null, rB = null;
            for (int i = 1; i <= 3; i++)
            {
                rA += collection["txtRunsA_" + i];
                rB += collection["txtRunsB_" + i];
                if (i != 3)
                {
                    rA += ",";
                    rB += ",";
                }
            }
            for (int i = 4; i <= 5; i++)
            {
                if (!string.IsNullOrWhiteSpace(collection["txtRunsA_" + i]))
                {
                    rA += "," + collection["txtRunsA_" + i];
                }
                if (!string.IsNullOrWhiteSpace(collection["txtRunsB_" + i]))
                {
                    rB += "," + collection["txtRunsB_" + i];
                }
            }
            ih.RunsA = rA;
            ih.RunsB = rB;
            if (_IIceHockeySchedulesService.EditScoreByIHBF(ih) > 0)
            {
                return Json("成功"); ;
            }
            else
                return Json("失敗");
        }

        public ActionResult DeleteScore(IceHockeySchedules ih)
        {
            if (_IIceHockeySchedulesService.DeleteScore(ih) > 0)
            {
                return Json("成功");
            }
            else
            {
                return Json("失败");
            }
        }
    }
}
