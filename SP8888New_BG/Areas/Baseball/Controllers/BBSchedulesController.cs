using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using IServices;
using Models;
using SP8888New_BG.Controllers;

namespace SP8888New_BG.Areas.Baseball.Controllers
{
    public class BBSchedulesController : BaseController
    {
        public IBaseballSchedulesService _IBaseballSchedulesService;
        public IBaseballAllianceService _IBaseballAllianceService;
        public IBaseballTeamService _IBaseballTeamService;
        private IScoreModifyRecordService _ISMRService;
        private IUser _user;

        public BBSchedulesController(IEmployeeService employeeService, IBaseballSchedulesService ibService, IBaseballAllianceService ia, IBaseballTeamService it, IScoreModifyRecordService ismrs)
            : base(employeeService)
        {
            _IBaseballSchedulesService = ibService;
            _IBaseballAllianceService = ia;
            _IBaseballTeamService = it;
            _ISMRService = ismrs;
            _user = employeeService.User;
        }

        //
        // GET: /Baseball/BBSchedules/
        public ActionResult Index(DateTime date, string gameType)
        {
            List<Models.ViewModel.Baseball> bb = _IBaseballSchedulesService.GetSchedules(date, gameType);

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "賽程資料" },
                HaveButton = false
            };

            ViewBag.ddlAllian = bb.Distinct(new Common.baseballComparer()).Select(p => new SelectListItem { Text = p.Alliance, Value = p.Alliance.Replace(" ", "").Replace(":", "") });
            ViewBag.date = date.ToString("yyyy-MM-dd");
            ViewBag.gameType = gameType;
            ViewBag.userName = _user.UserName;
            return View(bb);
        }

        /// <summary>
        /// 赛程列表存储设定
        /// </summary>
        [HttpPost]
        public ActionResult SetItem(IEnumerable<Models.ViewModel.Baseball> bb, SetItem si)
        {
            int c = _IBaseballSchedulesService.SaveSetting(bb, si);

            return Json(new { count = c });
        }

        //
        // GET: /Baseball/BBSchedules/Create
        /// <summary>
        ///  新增赛程主页
        /// </summary>
        public ActionResult Create(string gameType, DateTime GameDate, string wsbz = "")
        {
            ViewBag.IsAdd = true;
            ViewBag.GameType = gameType;
            ViewBag.msg = wsbz;
            var all = _IBaseballAllianceService.QueryByCondition(p => p.GameType == gameType && !p.IsDeleted);
            //大联盟
            ViewBag.DAllian = all.Where(p => p.Lever == 1).ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString() }).ToList();

            //默认选择第一个大联盟ID
            int dlm = all.Where(c => c.Lever == 1).First().AllianceID;
            //队伍联盟列表 第一个大联盟 + 此大联盟下级
            var teamAll = all.ToList().Where(p => (p.LeverOther != null && p.LeverOther.Contains(dlm.ToString())) || p.AllianceID == dlm);

            ViewBag.AllianListB = ViewBag.AllianListA = teamAll.ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString() }).ToList();

            //第一次加载队伍列表       
            ViewBag.TeamB = ViewBag.TeamA = _IBaseballTeamService.QueryByCondition(p => p.GameType == gameType && !p.IsDeleted).OrderBy(o => o.ShowName).ToList().Select(c => new SelectListItem { Text = c.ShowName, Value = c.TeamID.ToString() }).ToList();

            ViewBag.GameStates = AppData.GetAllGameStatus().Select(c => new SelectListItem { Text = c.StatusText, Value = c.Status }).ToList();

            ViewBag.CtrlStates = AppData.GetCtrlState().Select(c => new SelectListItem { Text = c.CtrlText, Value = c.CtrlValue }).ToList();

            BaseballSchedules bs = new BaseballSchedules();
            bs.Number = 0;
            bs.GameDate = GameDate.Date;
            bs.GameTime = new TimeSpan(00, 00, 00);
            bs.GameStates = "X";
            bs.IsReschedule = false;
            bs.IsDeleted = false;
            bs.ShowJS = true;
            bs.OrderBy = 1;
            bs.B = bs.S = bs.O = bs.Bases = 0;
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
        public ActionResult Create(BaseballSchedules bb, string gameType, string DDLGameStates, int DDLCtrlStates)
        {
            bb.GameStates = DDLGameStates;
            bb.CtrlStates = DDLCtrlStates;
            int c = _IBaseballSchedulesService.CreateSchedules(bb, gameType);
            if (c > 0)
            {
                return RedirectToAction("Index", new { date = bb.GameDate.ToString("yyyy-MM-dd"), gameType = gameType });
            }
            else
            {
                return RedirectToAction("Create", new { gameType = gameType, GameDate = bb.GameDate.ToString("yyyy-MM-dd"), wsbz = HttpUtility.UrlEncode(c == -3 ? "队伍选取重复" : c == -2 ? "大聯盟不存在" : c == 0 || c == -1 ? "隊伍不存在" : "新增賽事失敗") });
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
            List<BaseballAlliance> Alliancelist = _IBaseballAllianceService.QueryByCondition(p => p.GameType == gameType && (p.AllianceID == firstAllianceID || p.LeverOther.Contains(id))).ToList();
            List<BaseballTeam> team = _IBaseballTeamService.QueryByCondition(p => p.GameType == gameType && firstAllianceID == p.AllianceID).ToList();
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
            List<BaseballTeam> team = _IBaseballTeamService.QueryByCondition(p => p.GameType == gameType && otherAlliance == p.AllianceID).ToList();
            Dictionary<string, List<BaseballTeam>> dic = new Dictionary<string, List<BaseballTeam>>();
            dic.Add("team", team);
            return Json(dic);
        }


        /// <summary>
        /// 修改主页
        /// </summary>
        public ActionResult Edit(int GID, string wsbz = "")
        {
            BaseballSchedules bb = _IBaseballSchedulesService.QueryById(GID);
            ViewBag.IsAdd = false;
            ViewBag.GameType = bb.GameType;
            ViewBag.msg = wsbz;
            var all = _IBaseballAllianceService.QueryByCondition(p => p.GameType == bb.GameType && !p.IsDeleted);
            //大联盟
            ViewBag.DAllian = all.Where(p => p.Lever == 1).ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString(), Selected = c.AllianceID == bb.AllianceID }).ToList();

            //队伍联盟下拉框 
            var teamAll = all.ToList().Where(p => p.LeverOther.Contains(bb.AllianceID.ToString()) || p.AllianceID == bb.AllianceID);

            int iATeamAllianceID = _IBaseballTeamService.QueryById(bb.TeamAID).AllianceID;
            int iBTeamAllianceID = _IBaseballTeamService.QueryById(bb.TeamBID).AllianceID;

            ViewBag.AllianListA = teamAll.ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString(), Selected = c.AllianceID == iATeamAllianceID }).ToList();
            ViewBag.AllianListB = teamAll.ToList().Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString(), Selected = c.AllianceID == iBTeamAllianceID }).ToList();
            //加载队伍下拉框       
            ViewBag.TeamA = _IBaseballTeamService.QueryByCondition(p => p.GameType == bb.GameType && !p.IsDeleted).OrderBy(o => o.ShowName).ToList().Select(c => new SelectListItem { Text = c.ShowName, Value = c.TeamID.ToString(), Selected = c.TeamID == bb.TeamAID }).ToList();
            ViewBag.TeamB = _IBaseballTeamService.QueryByCondition(p => p.GameType == bb.GameType && !p.IsDeleted).OrderBy(o => o.ShowName).ToList().Select(c => new SelectListItem { Text = c.ShowName, Value = c.TeamID.ToString(), Selected = c.TeamID == bb.TeamBID }).ToList();

            ViewBag.GameStates = AppData.GetAllGameStatus().Select(c => new SelectListItem { Text = c.StatusText, Value = c.Status, Selected = c.Status.Equals(bb.GameStates) }).ToList();

            ViewBag.CtrlStates = AppData.GetCtrlState().Select(c => new SelectListItem { Text = c.CtrlText, Value = c.CtrlValue, Selected = c.CtrlValue.Equals(bb.CtrlStates.ToString()) }).ToList();

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(bb.GameType), "修改赛程" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", bb.GameDate.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", bb.GameType)
                },
                ButtonText = "返回賽程"
            };

            return View("SchedulesEdit", bb);
        }


        /// <summary>
        /// 修改赛程
        /// </summary>
        [HttpPost]
        public ActionResult Edit(int gid, BaseballSchedules bb, string DDLGameStates, int DDLCtrlStates)
        {
            bb.GameStates = DDLGameStates;
            bb.CtrlStates = DDLCtrlStates;
            int c = _IBaseballSchedulesService.EditSchedules(bb);
            if (c > 0)
            {
                return RedirectToAction("Index", new { date = bb.GameDate.ToString("yyyy-MM-dd"), gameType = bb.GameType });
            }
            else
            {
                return RedirectToAction("Edit", new { GID = bb.GID, wsbz = HttpUtility.UrlEncode(c == -3 ? "队伍选取重复" : c == -2 ? "大聯盟不存在" : c == 0 || c == -1 ? "隊伍不存在" : "修改賽事失敗") });
            }

        }

        /// <summary>
        /// 修改分数主页
        /// </summary>
        public ActionResult EditScore(int GID)
        {
            BaseballSchedules bb = _IBaseballSchedulesService.QueryById(GID);
            if (bb.GameStates == "X")
            {
                return RedirectToAction("Index", new { date = bb.GameDate.ToString("yyyy-MM-dd"), gameType = bb.GameType });
            }
            ViewBag.TeamAName = _IBaseballTeamService.QueryById(bb.TeamAID).ShowName;
            ViewBag.TeamBName = _IBaseballTeamService.QueryById(bb.TeamBID).ShowName;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(bb.GameType), "修改分數" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", bb.GameDate.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", bb.GameType)
                },
                ButtonText = "返回賽程"
            };
            return View("ScoreEdit", bb);
        }

        /// <summary>
        /// 修改分数
        /// </summary>
        [HttpPost]
        public ActionResult EditScore(BaseballSchedules bb)
        {
            int r = _IBaseballSchedulesService.EditScore(bb);
            if (r > 0)
            {
                return Json("成功"); ;
            }
            else
                return Json("失敗" + r);
        }

        public ActionResult ScoreModifyRecord(int GID, string GameType)
        {
            Models.ViewModel.Baseball b = _IBaseballSchedulesService.GetSchedulesByID(GID);
            if (b == null)
            {
                return null;
            }
            List<ScoreModifyRecord> list = _ISMRService.QueryByCondition(p => p.ScheduleID == GID && p.GameType == GameType).ToList();
            ViewBag.sAlliance = AppData.GetGameTypeName(b.GameType);
            ViewBag.GameDate = b.GameDate.ToString("yyyy-MM-dd  ") + b.GameTime.ToString(@"hh\:mm\:ss");
            ViewBag.dw = b.TeamA + " vs. " + b.TeamB;

            List<SelectListItem> arr = list.Select(p => p.ModifyTime.ToString("yyyy-MM-dd")).Distinct().ToList().Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            arr.Insert(0, new SelectListItem { Text = "全部", Value = "", Selected = true });
            ViewBag.ddlDate = arr;

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(GameType), "比分修改紀錄" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", b.GameDate.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", GameType)
                },
                ButtonText = "返回賽程"
            };

            return View(list);
        }

        #region 棒球操盘界面
        public ActionResult SchedulesFollow(int gid)
        {
            BaseballSchedules bb = _IBaseballSchedulesService.QueryById(gid);
            if (bb == null || !_IBaseballSchedulesService.CanCtrl(gid))
            {
                // 不是本人，不可變更
                return RedirectToAction("Index", new { date = bb.GameDate.ToString("yyyy-MM-dd"), gameType = bb.GameType });
            }
            bb.CtrlAdmin = _user.UserName;
            bb.CtrlStates = 1;
            _IBaseballSchedulesService.Update(bb);
            _IBaseballSchedulesService.Commit();

            ViewBag.teamA = _IBaseballTeamService.GetDataById(bb.TeamAID).ShowName;
            ViewBag.teamB = _IBaseballTeamService.GetDataById(bb.TeamBID).ShowName;

            ViewBag.GID = gid;
            ViewBag.GameType = bb.GameType;
            return View(bb);
        }

        [HttpPost]
        public ActionResult UpdateFollow(BaseballSchedules bb, int type)
        {
            bb.RunsA = bb.RunsA ?? "";
            bb.RunsB = bb.RunsB ?? "";
            bb.RunsA = string.Join(",", bb.RunsA.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            bb.RunsB = string.Join(",", bb.RunsB.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            if (!string.IsNullOrWhiteSpace(bb.TrackerText))
            {
                bb.TrackerText = HttpUtility.UrlDecode(bb.TrackerText);
            }
            if (_IBaseballSchedulesService.UpdateFollow(bb, type) > 0)
            {
                return Json("ok");
            }
            else
            {
                return Json("失败！");
            }

        }
        #endregion
    }
}
