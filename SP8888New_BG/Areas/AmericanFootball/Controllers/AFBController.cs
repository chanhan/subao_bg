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

namespace SP8888New_BG.Areas.AmericanFootball.Controllers
{
    public class AFBController : BaseController
    {
        private IAFBService _IAFBService;
        private IAFBAllianceService _IAFBAllianceService;
        private IScoreModifyRecordService _IScoreModifyRecordService;
        private ISourceTypeService _ISourceTypeService;
        private IAFBTeamService _IAFBTeamService;
        //
        // GET: /AmericanFootball/AFB/
        public AFBController(IAFBService afbService, IAFBAllianceService allianceService, IScoreModifyRecordService scoreModifyRecordService, ISourceTypeService sourceTypeService, IAFBTeamService afbTeamService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IAFBService = afbService;
            _IAFBAllianceService = allianceService;
            _IScoreModifyRecordService = scoreModifyRecordService;
            _ISourceTypeService = sourceTypeService;
            _IAFBTeamService = afbTeamService;
        }
        public ActionResult Index(string gameType, int? gid, DateTime date)
        {
            List<AFB> schedules = _IAFBService.GetAFBSchedules(gameType, gid, date);
            ViewBag.GameType = gameType;
            ViewBag.GID = gid == null ? "" : gid.Value.ToString();
            ViewBag.date = date;
            List<AFBAlliance> allianceList = schedules.Select(p => new AFBAlliance { AllianceID = p.AllianceID, AllianceName = p.AllianceName }).Distinct(new AFBAllianceComparer()).ToList();
            ViewBag.Alliances = allianceList.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() }).ToList();
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "賽程資料" },
                HaveButton = false
            };
            return View(schedules);
        }
        /// <summary>
        /// 走势显示设置
        /// </summary>
        /// <param name="shedules"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetShowJs(IList<AFBSchedules> shedules)
        {
            int n = _IAFBService.SetShowJs(shedules);
            return Json(n);

        }
        /// <summary>
        /// 赛事显示设置
        /// </summary>
        /// <param name="shedules"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetShow(IList<AFBSchedules> shedules)
        {
            int n = _IAFBService.SetShow(shedules);
            return Json(n);
        }

        public ActionResult EditSchedule(DateTime date, string gameType, int? gid)
        {
            //大联盟级别
            int firstLever = 1;
            //查找所有队伍、联盟
            List<AFBAllianceTeam> list = _IAFBAllianceService.GetAllianceAndTeam(gameType);
            //取得虽有大联盟
            List<AFBAllianceTeam> firstLevel = list.Where(p => p.Lever == firstLever).Distinct(new AFBAllianceTeamComparer()).ToList();
            AFB afb = new AFB();
            //新增赛程
            if (gid == null)
            {
                int allianceID = 1;
                //取得第一个大联盟AllianceID
                AFBAllianceTeam addTeam = firstLevel.FirstOrDefault();
                if (addTeam != null)
                {
                    allianceID = addTeam.AllianceID.Value;
                }
                //新增设定默认值
                afb = new AFB
                {
                    AllianceID = allianceID,
                    GameDate = date.Date,
                    GameTime =  new TimeSpan(0,0,0),
                    ShowJS = true
                };
                ViewBag.firstLevel = firstLevel.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() });
                //大联盟下的其他联盟
                List<AFBAllianceTeam> otherLevel = list.Where(p => p.AllianceID == afb.AllianceID || p.LeverOther.Split('*').ToList().Contains(afb.AllianceID.ToString())).Distinct(new AFBAllianceTeamComparer()).ToList();
                ViewBag.otherLevela = otherLevel.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() }).ToList();
                ViewBag.otherLevelb = otherLevel.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() }).ToList();
                //联盟下的队伍
                List<AFBAllianceTeam> team = list.Where(p => (p.AllianceID == afb.AllianceID || p.LeverOther.Split('*').ToList().Contains(afb.AllianceID.ToString())) && p.ShowName != null).OrderBy(p => p.ShowName).ToList();
                ViewBag.teama = team.Select(p => new SelectListItem { Text = p.ShowName, Value = p.TeamID.ToString() }).ToList();
                ViewBag.teamb = team.Select(p => new SelectListItem { Text = p.ShowName, Value = p.TeamID.ToString() }).ToList();
                ViewBag.GameStates = AppData.GetAllGameStatus().Select(c => new SelectListItem { Text = c.StatusText, Value = c.Status }).ToList();
                ViewBag.CtrlStates = AppData.GetCtrlState(true).Select(c => new SelectListItem { Text = c.CtrlText, Value = c.CtrlValue }).ToList();
                ViewBag.IsAdd = true;
            }
            //修改
            else
            {
                //取得赛事信息
                afb = _IAFBService.GetAFBSchedules(gameType, gid, date).SingleOrDefault();
                //选中对应大联盟
                ViewBag.firstLevel = firstLevel.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString(), Selected = p.AllianceID.Value == afb.AllianceID }).ToList();
                List<AFBAllianceTeam> otherLevel = list.Where(p => p.AllianceID == afb.AllianceID || p.LeverOther.Split('*').ToList().Contains(afb.AllianceID.ToString())).Distinct(new AFBAllianceTeamComparer()).ToList();
                //选中对应大联盟下的其他联盟               
                ViewBag.otherLevela = otherLevel.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString(), Selected = p.AllianceID.Value == afb.TeamAAllianceID });
                ViewBag.otherLevelb = otherLevel.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString(), Selected = p.AllianceID.Value == afb.TeamBAllianceID });
                //联盟下的队伍               
                List<AFBAllianceTeam> teama = list.Where(p => (p.AllianceID == afb.TeamAAllianceID || p.LeverOther.Split('*').ToList().Contains(afb.TeamAAllianceID.ToString())) && p.ShowName != null).OrderBy(p => p.ShowName).ToList();
                List<AFBAllianceTeam> teamb = list.Where(p => (p.AllianceID == afb.TeamBAllianceID || p.LeverOther.Split('*').ToList().Contains(afb.TeamBAllianceID.ToString())) && p.ShowName != null).OrderBy(p => p.ShowName).ToList();
                //选中队伍
                ViewBag.teama = teama.Select(p => new SelectListItem { Text = p.ShowName, Value = p.TeamID.ToString(), Selected = p.TeamID == afb.TeamAID }).ToList();
                ViewBag.teamb = teamb.Select(p => new SelectListItem { Text = p.ShowName, Value = p.TeamID.ToString(), Selected = p.TeamID == afb.TeamBID }).ToList();
                //选中比赛状态
                ViewBag.GameStates = AppData.GetAllGameStatus().Select(c => new SelectListItem { Text = c.StatusText, Value = c.Status, Selected = c.Status == afb.GameStates }).ToList();
                //选中操盘状态                
                ViewBag.CtrlStates = AppData.GetCtrlState(true).Select(c => new SelectListItem { Text = c.CtrlText, Value = c.CtrlValue, Selected = c.CtrlValue == afb.CtrlStates.ToString() }).ToList();
             //   ViewBag.GameSource = basketball.GameSource;
                ViewBag.IsAdd = false;
            }
            ViewBag.GameType = gameType;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), gid == null ? "賽程新增" : "賽程修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("gameType", gameType),
                    new Parameter("date", date.ToString("yyyy-MM-dd")),
                }
            };
            return View(afb);
        }
        /// <summary>
        /// 切换大联盟，查询其下联盟、队伍
        /// </summary>
        /// <param name="firstAllianceID"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeFirstAlliance(int firstAllianceID, string gameType)
        {
            List<AFBAllianceTeam> list = _IAFBAllianceService.GetAllianceAndTeam(gameType);
            List<AFBAllianceTeam> otherLevel = list.Where(p => p.AllianceID == firstAllianceID || p.LeverOther.Split('*').ToList().Contains(firstAllianceID.ToString())).Distinct(new AFBAllianceTeamComparer()).ToList();
            List<AFBAllianceTeam> team = list.Where(p => (p.AllianceID == firstAllianceID || p.LeverOther.Split('*').ToList().Contains(firstAllianceID.ToString())) && p.ShowName != null).OrderBy(p => p.ShowName).ToList();
            return Json(new { alliance = otherLevel, team = team });
        }
        /// <summary>
        ///  切换其他联盟，查询其下队伍
        /// </summary>
        /// <param name="otherAlliance"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeOtherAlliance(int otherAlliance, string gameType)
        {
            List<AFBAllianceTeam> list = _IAFBAllianceService.GetAllianceAndTeam(gameType);
            List<AFBAllianceTeam> team = list.Where(p => (p.AllianceID == otherAlliance || p.LeverOther.Split('*').ToList().Contains(otherAlliance.ToString())) && p.ShowName != null).OrderBy(p => p.ShowName).ToList();
            return Json(new { team = team });
        }
        [HttpPost]
        public ActionResult EditSchedule(AFBSchedules basketball)
        {
            int count = _IAFBService.EditSchedule(basketball);
            return Json(count);
        }
        /// <summary>
        /// 分数修改
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="gameType"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult UpdateScore(int gid, string gameType, DateTime date, int? index)
        {
            AFBScoreModify afb = _IAFBService.GetModifySchedules(gid);
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "分數修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("gameType", gameType),
                    new Parameter("date", date.ToString("yyyy-MM-dd")),
                }
            };
            ViewBag.index = index;
            ViewBag.gameType = gameType;
            ViewBag.gameDate = date;
            ViewBag.gid = gid;
            return View(afb);
        }
        [HttpPost]
        public ActionResult UpdateScore(AFBSchedules afb)
        {
            int n = _IAFBService.UpdateScore(afb);
            return Json(n);
        }
        public ActionResult ScoreModifyRecord(int gid, DateTime date, string gameType)
        {
            List<AFBScoreRecord> list = _IScoreModifyRecordService.GetAFBScoreModifyRecord(gid, gameType);
            ViewBag.AFB = _IAFBService.GetAFBSchedules(gameType, gid, date).SingleOrDefault();
            ViewBag.Date = list.Select(p => new SelectListItem { Text = p.ModifyTime.ToString("yyyy-MM-dd"), Value = p.ModifyTime.ToString("yyyyMMdd") }).Distinct(new SelectListItemComparer());
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "比分修改紀錄" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("gameType", gameType),
                    new Parameter("date",date.ToString("yyyy-MM-dd")),
                }
            };
            return View(list);
        }
        public ActionResult AFBAlliance(string gameType)
        {
            List<AFBAlliance> alliance = _IAFBAllianceService.QueryByCondition(p => p.GameType == gameType).OrderBy(p => p.AllianceID).ToList();
            ViewBag.FirstAlliance = alliance.Where(p => p.Lever == 1).Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() });
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "聯盟管理" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("gameType", gameType),
                    new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                }
            };
            ViewBag.GameType = gameType;
            return View(alliance);
        }
        /// <summary>
        /// 联盟新增、修改
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="allianceid"></param>
        /// <returns></returns>
        public ActionResult UpdateAlliance(string gameType, int? allianceid)
        {
            List<AFBAlliance> alliances = _IAFBAllianceService.QueryByCondition(p => p.GameType == gameType).OrderBy(p => p.Lever).ToList();
            AFBAlliance alliance = new AFBAlliance { GameType = gameType };
            if (allianceid != null)
            {
                alliance = alliances.SingleOrDefault(p => p.AllianceID == allianceid);
            }
            ViewBag.FirstAlliance = alliances.Where(p => p.Lever == 1).Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() });
            ViewBag.SecondAlliance = alliances.Where(p => p.Lever == 2).Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() });
            ViewBag.IsAdd = allianceid == null;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), allianceid == null ? "聯盟新增" : "聯盟修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "AFBAlliance",
                HaveButton = true,
                ButtonText = "回聯盟管理",
                Parameter = new List<Parameter>{
                    new Parameter("gameType", gameType)
                }
            };
            return View(alliance);
        }
        [HttpPost]
        public ActionResult UpdateAlliance(AFBAlliance alliance, bool isAdd)
        {
            int n = _IAFBAllianceService.UpdateAlliance(alliance, isAdd);
            return Json(n);
        }
        /// <summary>
        /// 切换大联盟
        /// </summary>
        /// <param name="firstAllianceID"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateFirstAlliance(int firstAllianceID, string gameType)
        {
            string allianceIdStr = string.Format("{0}{1}{0}", "*", firstAllianceID);
            List<AFBAlliance> alliance = _IAFBAllianceService.QueryByCondition(p => (p.Lever == 1 || p.Lever == 2) && allianceIdStr.IndexOf(p.LeverOther) >= 0).ToList();
            return Json(alliance);
        }
        /// <summary>
        /// 切换二联盟
        /// </summary>
        /// <param name="secondAllianceID"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateSecondAlliance(int secondAllianceID, string gameType)
        {
            AFBAllianceTeam alliance = _IAFBAllianceService.GetParentAlliance(gameType, secondAllianceID);
            return Json(alliance);
        }
        /// <summary>
        /// 隊伍管理
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        public ActionResult AFBTeam(string gameType)
        {
            List<AFBAllianceTeam> team = _IAFBTeamService.GetAFBTeam(gameType);
            ViewBag.alliance = team.Distinct(new AFBAllianceTeamComparer()).Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() }).ToList();
            ViewBag.sourceType = _ISourceTypeService.GetSourceType(gameType).Distinct(new SourceTypeComparer()).Select(p => new SelectListItem { Text = p.GameSource, Value = p.SourceID }).ToList();
            ViewBag.GameType = gameType;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "隊伍管理" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("gameType", gameType),
                    new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                }
            };
            return View(team);
        }
        /// <summary>
        /// 删除队伍
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTeam(string gameType, int teamid)
        {
            int n = _IAFBTeamService.DeleteTeam(gameType, teamid);
            return Json(n);
        }
        /// <summary>
        /// 切换联盟，查找来源网
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="allianceID"></param>
        /// <returns></returns>
        public ActionResult ChangeSource(string gameType, int? allianceID)
        {
            List<SourceType> list = _ISourceTypeService.GetSourceType(gameType, allianceID).Distinct(new SourceTypeComparer()).ToList();
            return Json(list);
        }
        /// <summary>
        /// 队伍新增、修改
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        public ActionResult UpdateTeam(string gameType, int? teamid)
        {
            AFBTeam team = new AFBTeam();
            string imagePath = string.Empty;
            if (teamid != null)
            {
                team = _IAFBTeamService.QueryById(teamid.Value);
                imagePath = AppData.GetTeamImagePath(gameType, teamid.Value);
                imagePath = System.IO.File.Exists(Server.MapPath(imagePath)) ? imagePath : string.Empty;
            }
            List<AFBAlliance> alliances = _IAFBAllianceService.QueryByCondition(p => p.GameType == gameType).ToList();
            ViewBag.alliance = alliances.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString(), Selected = teamid == null ? false : p.AllianceID == team.AllianceID });
            ViewBag.ExistImage = imagePath;
            ViewBag.IsAdd = teamid == null;
            ViewBag.gameType = gameType;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "隊伍修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "AFBTeam",
                HaveButton = true,
                ButtonText = "回隊伍管理",
                Parameter = new List<Parameter>{
                    new Parameter("gameType", gameType)
                }
            };
            return View(team);
        }

        [HttpPost]
        public ActionResult UpdateTeam(AFBTeam team, HttpPostedFileBase TeamImage, bool isAdd)
        {
            team.WebName = string.IsNullOrEmpty(team.WebName) ? null : team.WebName.Replace("\r\n", ",");
            int n = _IAFBTeamService.UpdateTeam(team, isAdd);
            if (TeamImage != null)
            {
                int id = team.TeamID;
                AppData.UpLoadImage(Server.MapPath(AppData.GetImagePath(team.GameType)), TeamImage.InputStream, Server.MapPath(AppData.GetTeamImagePath(team.GameType, id)));
                //添加随机数 刷新图片
                return Json(new { count = n, path = string.Format("{0}?{1}", AppData.GetTeamImagePath(team.GameType, id), DateTime.Now.Ticks) });
            }
            else
            {
                return Json(new { count = n });
            }
        }
    }
}
