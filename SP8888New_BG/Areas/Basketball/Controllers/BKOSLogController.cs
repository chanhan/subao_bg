using IServices;
using Models;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.Basketball.Controllers
{
    public class BKOSLogController : BaseController
    {
        //
        // GET: /Basketball/BKOSLog/

        private IBKOSService _IBKOSService;
        private IBKOSAllianceService _IBKOSAllianceService;
        private IBKOSTeamService _IBKOSTeamService;
        private IBasketballAllianceService _IBasketballAllianceService;
        private IBasketballTeamService _IBasketballTeamService;
        private IOSTeamService _IOSTeamService;
        public BKOSLogController(IBKOSService bkosService, IBKOSAllianceService allianceService, IBKOSTeamService teamService, IBasketballAllianceService basketballalliance, IBasketballTeamService basketballteam, IOSTeamService osteamservice, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IBKOSService = bkosService;
            _IBKOSAllianceService = allianceService;
            _IBKOSTeamService = teamService;
            _IBasketballAllianceService = basketballalliance;
            _IBasketballTeamService = basketballteam;
            _IOSTeamService = osteamservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Schedule(IEnumerable<ModifyRecord> records)
        {
            List<BasketBall> oldSchedules = new List<BasketBall>();
            List<BasketBall> newSchedules = new List<BasketBall>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BasketballSchedules old = _IBKOSService.JsonDeserialize(p.OldData);
                BasketballSchedules New = _IBKOSService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldSchedules.Add(new BasketBall
                    {
                        AllianceName = _IBKOSAllianceService.GetDataById(old.AllianceID),
                        GameDate = old.GameDate,
                        GameTime = old.GameTime,
                        GameType = old.GameType,
                        TeamAName = _IBKOSTeamService.GetDataById(old.TeamAID),
                        TeamBName = _IBKOSTeamService.GetDataById(old.TeamBID),
                        GameStates = old.GameStates,
                        CtrlStates = old.CtrlStates,
                        CtrlAdmin = old.CtrlAdmin,
                        WebID = old.WebID,
                        TrackerText = old.TrackerText,
                        ShowJS = old.ShowJS,
                        IsDeleted = old.IsDeleted
                    });
                }
                if (New != null)
                {
                    newSchedules.Add(new BasketBall
                    {
                        AllianceName = _IBKOSAllianceService.GetDataById(New.AllianceID),
                        GameDate = New.GameDate,
                        GameTime = New.GameTime,
                        GameType = New.GameType,
                        TeamAName = _IBKOSTeamService.GetDataById(New.TeamAID),
                        TeamBName = _IBKOSTeamService.GetDataById(New.TeamBID),
                        GameStates = New.GameStates,
                        CtrlStates = New.CtrlStates,
                        CtrlAdmin = New.CtrlAdmin,
                        WebID = New.WebID,
                        TrackerText = New.TrackerText,
                        ShowJS = New.ShowJS,
                        IsDeleted  =New.IsDeleted
                    });
                }
            });
            return View(Tuple.Create(oldSchedules, newSchedules, list[0].ActionStatus));
        }

        /// <summary>
        /// 联盟
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult Alliance(IEnumerable<ModifyRecord> records)
        {
            List<BKOSAlliance> oldAlliance = new List<BKOSAlliance>();
            List<BKOSAlliance> newAlliance = new List<BKOSAlliance>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BKOSAlliance old = _IBKOSAllianceService.JsonDeserialize(p.OldData);
                BKOSAlliance New = _IBKOSAllianceService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldAlliance.Add(old);
                }
                if (New != null)
                {
                    newAlliance.Add(New);
                }
            });
            return View(Tuple.Create(oldAlliance, newAlliance, list[0].ActionStatus));
        }

        /// <summary>
        /// 队伍
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult Team(IEnumerable<ModifyRecord> records)
        {
            List<BKOSTeam> oldTeam = new List<BKOSTeam>();
            List<BKOSTeam> newTeam = new List<BKOSTeam>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                OSTeam old = _IOSTeamService.JsonDeserialize(p.OldData);
                OSTeam New = _IOSTeamService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldTeam.Add(new BKOSTeam
                    {
                        AllianceName = _IBKOSAllianceService.GetDataById(old.AllianceID),
                       ShowName = old.ShowName,
                       TeamName = old.TeamName
                    });
                }
                if (New != null)
                {
                    newTeam.Add(new BKOSTeam
                    {
                        AllianceName = _IBKOSAllianceService.GetDataById(New.AllianceID),
                        ShowName = New.ShowName,
                        TeamName = New.TeamName
                    });
                }
            });
            return View(Tuple.Create(oldTeam, newTeam, list[0].ActionStatus));
        }
    }
}
