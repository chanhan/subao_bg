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
using PagedList;
using Helper.Pager;
using Models.QueryModel;
namespace SP8888New_BG.Areas.Basketball.Controllers
{
    public class BKOSController : BaseController
    {
        //
        // GET: /Basketball/BKOS/

        private IBKOSService _IBKOSService;
        private IScoreModifyRecordService _IScoreModifyRecordService;
        private IBKOSAllianceService _IBKOSAllianceService;
        private IBKOSTeamService _IBKOSTeamService;
        private IOSTeamService _IOSTeamService;
        public BKOSController(IBKOSService bkosService, IScoreModifyRecordService scoreModifyRecordService, IBKOSAllianceService allianceService, IBKOSTeamService teamService, IOSTeamService osTeamService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IBKOSService = bkosService;
            _IScoreModifyRecordService = scoreModifyRecordService;
            _IBKOSAllianceService = allianceService;
            _IBKOSTeamService = teamService;
            _IOSTeamService = osTeamService;
        }

        public ActionResult Index(DateTime date)
        {
            List<BKOSSchedules> schedules = _IBKOSService.GetBKOSSchedules(date);
            List<BKOSAlliance> alliances = schedules.Select(p => new BKOSAlliance { AllianceID = p.AllianceID, AllianceName = p.AllianceName, Display = p.AllianceDisPlay }).Distinct(new BKOSAllianceComparer()).ToList();
            ViewBag.Alliances = alliances;
            ViewBag.SelectAlliances = alliances.Select(c => new SelectListItem { Text = c.AllianceName, Value = c.AllianceID.ToString() });
            ViewBag.date = date.ToString("yyyy-MM-dd");
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "奧訊籃球", "賽程資料" },
                HaveButton = false
            };
            return View(schedules);
        }

        /// <summary>
        /// 存储设定
        /// </summary>
        /// <param name="bkos"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(IList<BasketballSchedules> bkos)
        {
            int n = _IBKOSService.SaveSchedules(bkos);
            return Json(n);
            //  return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);  
        }

        public ActionResult UpdateScore(int gid)
        {
            BKOSScoreModify bkos = _IBKOSService.GetModifySchedules(gid);
            ViewBag.GameStatus = AppData.GetGameStatus().Select(c => new SelectListItem { Text = c.StatusText, Value = c.Status, Selected = bkos.GameStates == c.Status });
            ViewBag.ModifyItem = AppData.GetModifyItems().Select(c => new SelectListItem { Text = c.ItemText, Value = c.ItemValue.ToString(), Selected = bkos.CtrlStates == c.ItemValue });
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "奧訊籃球", "分數修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("date", bkos.Date.ToString("yyyy-MM-dd")),
                }
            };
            return View(bkos);
        }

        [HttpPost]
        public ActionResult UpdateScore(BasketballSchedules basketball, int modifyItem)
        {
            int n = _IBKOSService.UpdateScore(basketball, modifyItem);
            return Json(n);
        }

        [HttpPost]
        public ActionResult DeleteScore(int gid)
        {
            int n = _IBKOSService.DeleteScore(gid);
            return Json(n);
        }
        public ActionResult ScoreModifyRecord(int gid, DateTime date)
        {
            List<BKOSScoreRecord> list = _IScoreModifyRecordService.GetScoreModifyRecord(gid, "BKOS");
            ViewBag.BKOS = _IBKOSService.GetBKOSScheduleByID(gid);
            ViewBag.Date = list.Select(p => new SelectListItem { Text = p.ModifyTime.ToString("yyyy-MM-dd"), Value = p.ModifyTime.ToString("yyyyMMdd") }).Distinct(new SelectListItemComparer());
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "奧訊籃球", "比分修改紀錄" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("date",date.ToString("yyyy-MM-dd")),
                }
            };
            return View(list);
        }
        public ActionResult BKOSAlliance(string keyWords, int pageIndex = 1, int pageSize = 20)
        {
            int count = 0;
            List<BKOSAlliance> pagedList = _IBKOSAllianceService.GetBKOSAllianceByCondition(keyWords, pageIndex, pageSize, out count).ToList();
            PagerInfo pager = new PagerInfo(pageIndex, pageSize, count);
            PagerQuery<PagerInfo, List<BKOSAlliance>, string> query = new PagerQuery<PagerInfo, List<BKOSAlliance>, string>(pager, pagedList, keyWords);
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "奧訊籃球", "聯盟管理" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("date",DateTime.Now.ToString("yyyy-MM-dd")),
                }
            };
            return View(query);
        }

        [HttpPost]
        public ActionResult BKOSAlliance(IList<BKOSAlliance> alliance)
        {
            int n = _IBKOSAllianceService.AllianceDispalySet(alliance);
            return Json(n);
        }

        [HttpPost]
        public ActionResult UpdateAlliance(BKOSAlliance alliance)
        {
            int n=  _IBKOSAllianceService.UpdateAlliance(alliance);
            return Json(n);
        }

        public ActionResult BKOSTeam(BKOSTeamQuery queryModel, int pageIndex = 1, int pageSize = 100)
        {
            int count = 0;
            List<BKOSTeam> pagedList = _IBKOSTeamService.GetBKOSTeamByCondition(queryModel, pageIndex, pageSize, out count).ToList();
            PagerInfo pager = new PagerInfo(pageIndex, pageSize, count);
            PagerQuery<PagerInfo, List<BKOSTeam>, BKOSTeamQuery> query = new PagerQuery<PagerInfo, List<BKOSTeam>, BKOSTeamQuery>(pager, pagedList, queryModel);
            ViewBag.SelectAlliances = _IBKOSAllianceService.QueryAll().ToList().Select(p => new SelectListItem { Text = p.ShowName, Value = p.AllianceID.ToString(), Selected = queryModel != null && queryModel.AllianceID == p.AllianceID });
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "奧訊籃球", "隊伍管理" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                ButtonText = "回賽程頁",
                Parameter = new List<Parameter>{
                    new Parameter("date",DateTime.Now.ToString("yyyy-MM-dd")),
                }
            };
            return View(query);
        }

        [HttpPost]
        public ActionResult UpdateTeam(OSTeam team)
        {
            int n = _IOSTeamService.UpdateTeam(team);
            return Json(n);
        }
    }
}
