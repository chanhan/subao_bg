using Common;
using IServices;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using SP8888New_BG.Controllers;

namespace SP8888New_BG.Areas.IceHockey.Controllers
{
    public class IHSchedulesController : BaseController
    {
        private IIceHockeySchedulesService _IIceHockeySchedulesService;
        private IIceHockeyAllianceService _IIceHockeyAllianceService;
        private IIceHockeyTeamService _IIceHockeyTeamService;
        private IScoreModifyRecordService _ISMRService;

        public IHSchedulesController(IEmployeeService employeeService, IIceHockeySchedulesService ihService, IIceHockeyAllianceService ia, IIceHockeyTeamService it, IScoreModifyRecordService ismrs)
            : base(employeeService)
        {
            _IIceHockeySchedulesService = ihService;
            _IIceHockeyAllianceService = ia;
            _IIceHockeyTeamService = it;
            _ISMRService = ismrs;
        }

        public ActionResult Index(DateTime date, string gameType)
        {

            List<Models.ViewModel.IceHockey> ih = _IIceHockeySchedulesService.GetSchedules(date, gameType);



            ViewBag.ddlAllian = ih.Distinct(new Common.IceHockeyComparer()).Select(p => new SelectListItem { Text = p.Alliance, Value = p.Alliance.Replace(" ", "").Replace(":", "") }).ToList();

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
            return View(ih);
        }

        /// <summary>
        /// 赛程列表存储设定
        /// </summary>
        [HttpPost]
        public ActionResult SetItem(IEnumerable<Models.ViewModel.IceHockey> ih, SetItem si)
        {
            int c = _IIceHockeySchedulesService.SaveSetting(ih, si);

            return Json(new { count = c });
        }

        //
        // GET: /IceHockey/BBSchedules/Create
        /// <summary>
        ///  新增赛程主页
        /// </summary>
        public ActionResult Create(string gameType, DateTime GameDate, string wsbz = "")
        {
            ViewBag.IsAdd = true;
            ViewBag.GameType = gameType;
            ViewBag.msg = wsbz;
            var all = _IIceHockeyAllianceService.QueryByCondition(p => p.GameType == gameType && p.Display);
            //大联盟
            ViewBag.DAllian = all.Where(p => p.Lever == 1).ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString() }).ToList();

            //默认选择第一个大联盟ID
            int dlm = all.Where(c => c.Lever == 1).First().AllianceID;
            //队伍联盟列表 第一个大联盟 + 此大联盟下级
            var teamAll = all.ToList().Where(p => p.LeverOther.Contains(dlm.ToString()) || p.AllianceID == dlm);

            ViewBag.AllianListB = ViewBag.AllianListA = teamAll.ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString() }).ToList();

            //第一次加载队伍列表       
            ViewBag.TeamB = ViewBag.TeamA = _IIceHockeyTeamService.QueryByCondition(p => p.GameType == gameType && p.Display).OrderBy(o => o.ShowName).ToList().Select(c => new SelectListItem { Text = c.ShowName, Value = c.TeamID.ToString() }).ToList();

            ViewBag.GameStates = AppData.GetAllGameStatus().Select(c => new SelectListItem { Text = c.StatusText, Value = c.Status }).ToList();

            ViewBag.CtrlStates = AppData.GetCtrlState().Select(c => new SelectListItem { Text = c.CtrlText, Value = c.CtrlValue }).ToList();

            IceHockeySchedules bs = new IceHockeySchedules();
            bs.Number = 0;
            bs.GameDate = GameDate.Date;
            bs.GameTime = new TimeSpan(00, 00, 00);
            bs.GameStates = "X";
            bs.ShowJS = true;
            bs.OrderBy = 0;

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "新增赛程" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", GameDate.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", gameType)
                },
                ButtonText = "返回賽程"
            };

            return View("SchedulesEdit", bs);
        }

        /// <summary>
        /// 新增赛程
        /// </summary>
        [HttpPost]
        public ActionResult Create(IceHockeySchedules ih, string gameType, string DDLGameStates, int DDLCtrlStates)
        {
            ih.GameStates = DDLGameStates;
            ih.CtrlStates = DDLCtrlStates;
            ih.Display = true;
            int c = _IIceHockeySchedulesService.CreateSchedules(ih, gameType);
            if (c > 0)
            {
                return RedirectToAction("Index", new { date = ih.GameDate.ToString("yyyy-MM-dd"), gameType = gameType });
            }
            else
            {
                return RedirectToAction("Create", new { gameType = gameType, GameDate = ih.GameDate.ToString("yyyy-MM-dd"), wsbz = HttpUtility.UrlEncode(c == -3 ? "队伍选取重复" : c == -2 ? "大聯盟不存在" : c == 0 || c == -1 ? "隊伍不存在" : "修改賽事失敗") });
            }
        }


        /// <summary>
        /// 切换大联盟
        /// </summary>
        /// <param name="firstAllianceID">选中联盟</param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeFirstAlliance(int firstAllianceID, string gameType)
        {
            string id = firstAllianceID.ToString();
            List<IceHockeyAlliance> Alliancelist = _IIceHockeyAllianceService.QueryByCondition(p => p.GameType == gameType && (p.AllianceID == firstAllianceID || p.LeverOther.Contains(id))).ToList();
            List<IceHockeyTeam> team = _IIceHockeyTeamService.QueryByCondition(p => p.GameType == gameType && firstAllianceID == p.AllianceID).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("alliance", Alliancelist);
            dic.Add("team", team);
            return Json(dic);
        }
        /// <summary>
        /// 切换队伍联盟
        /// </summary>
        [HttpPost]
        public ActionResult ChangeOtherAlliance(int otherAlliance, string gameType)
        {
            List<IceHockeyTeam> team = _IIceHockeyTeamService.QueryByCondition(p => p.GameType == gameType && otherAlliance == p.AllianceID).ToList();
            Dictionary<string, List<IceHockeyTeam>> dic = new Dictionary<string, List<IceHockeyTeam>>();
            dic.Add("team", team);
            return Json(dic);
        }


        /// <summary>
        /// 修改主页
        /// </summary>
        public ActionResult Edit(int GID, string wsbz = "")
        {
            IceHockeySchedules ih = _IIceHockeySchedulesService.QueryById(GID);
            ViewBag.IsAdd = false;
            ViewBag.GameType = ih.GameType;
            ViewBag.msg = wsbz;
            var all = _IIceHockeyAllianceService.QueryByCondition(p => p.GameType == ih.GameType && p.Display);
            //大联盟
            ViewBag.DAllian = all.Where(p => p.Lever == 1).ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString(), Selected = c.AllianceID == ih.AllianceID }).ToList();

            //队伍联盟下拉框 
            var teamAll = all.ToList().Where(p => p.LeverOther.Contains(ih.AllianceID.ToString()) || p.AllianceID == ih.AllianceID);

            int iATeamAllianceID = _IIceHockeyTeamService.QueryById(ih.TeamAID).AllianceID;
            int iBTeamAllianceID = _IIceHockeyTeamService.QueryById(ih.TeamBID).AllianceID;

            ViewBag.AllianListA = teamAll.ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString(), Selected = c.AllianceID == iATeamAllianceID }).ToList();
            ViewBag.AllianListB = teamAll.ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString(), Selected = c.AllianceID == iBTeamAllianceID }).ToList();
            //加载队伍下拉框       
            ViewBag.TeamA = _IIceHockeyTeamService.QueryByCondition(p => p.GameType == ih.GameType && p.Display).OrderBy(o => o.ShowName).ToList().Select(c => new SelectListItem { Text = c.ShowName, Value = c.TeamID.ToString(), Selected = c.TeamID == ih.TeamAID }).ToList();
            ViewBag.TeamB = _IIceHockeyTeamService.QueryByCondition(p => p.GameType == ih.GameType && p.Display).OrderBy(o => o.ShowName).ToList().Select(c => new SelectListItem { Text = c.ShowName, Value = c.TeamID.ToString(), Selected = c.TeamID == ih.TeamBID }).ToList();

            ViewBag.GameStates = AppData.GetAllGameStatus().Select(c => new SelectListItem { Text = c.StatusText, Value = c.Status, Selected = c.Status.Equals(ih.GameStates) }).ToList();

            ViewBag.CtrlStates = AppData.GetCtrlState().Select(c => new SelectListItem { Text = c.CtrlText, Value = c.CtrlValue, Selected = c.CtrlValue.Equals(ih.CtrlStates.ToString()) }).ToList();

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(ih.GameType), "修改赛程" },
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

            return View("SchedulesEdit", ih);
        }


        /// <summary>
        /// 修改赛程
        /// </summary>
        [HttpPost]
        public ActionResult Edit(int gid, IceHockeySchedules ih, string DDLGameStates, int DDLCtrlStates)
        {
            ih.GameStates = DDLGameStates;
            ih.CtrlStates = DDLCtrlStates;
            int c = _IIceHockeySchedulesService.EditSchedules(ih);
            if (c > 0)
            {
                return RedirectToAction("Index", new { date = ih.GameDate.ToString("yyyy-MM-dd"), gameType = ih.GameType });
            }
            else
            {
                return RedirectToAction("Edit", new { GID = ih.GID, wsbz = HttpUtility.UrlEncode(c == -3 ? "队伍选取重复" : c == -2 ? "大聯盟不存在" : c == 0 || c == -1 ? "隊伍不存在" : "修改賽事失敗") });
            }

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
        public ActionResult EditScore(IceHockeySchedules ih)
        {
            int r = _IIceHockeySchedulesService.EditScore(ih);
            if (r > 0)
            {
                return Json("成功"); ;
            }
            else
                return Json("失敗" + r);
        }

        public ActionResult ScoreModifyRecord(int GID, string GameType)
        {
            Models.ViewModel.IceHockey ih = _IIceHockeySchedulesService.GetSchedulesByID(GID);
            if (ih == null)
            {
                return null;
            }
            List<ScoreModifyRecord> list = _ISMRService.QueryByCondition(p => p.ScheduleID == GID && p.GameType == GameType).ToList();
            ViewBag.sAlliance = AppData.GetGameTypeName(ih.GameType);
            ViewBag.GameDate = ih.GameDate.ToString("yyyy-MM-dd ") + ih.GameTime.ToString(@"hh\:mm\:ss");
            ViewBag.dw = ih.TeamA + " vs. " + ih.TeamB;

            List<SelectListItem> arr = list.Select(p => p.ModifyTime.ToString("yyyy-MM-dd")).Distinct().ToList().Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            arr.Insert(0, new SelectListItem { Text = "全部", Value = "", Selected = true });
            ViewBag.ddlDate = arr;
            ViewBag.isBF = (GameType == "IHBF");

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(GameType), "比分修改紀錄" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = GameType == "IHBF" ? "IHBFSchedules" : "IHSchedules",
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", ih.GameDate.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", GameType)
                },
                ButtonText = "返回賽程"
            };

            return View(list);
        }
    }
}
