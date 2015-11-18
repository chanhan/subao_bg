using IServices;
using Models;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.AmericanFootball.Controllers
{
    public class LogController : BaseController
    {
        private IAFBService _IAFBService;
        private IAFBAllianceService _IAFBAllianceService;
        private IAFBTeamService _IAFBTeamService;
        public LogController(IAFBService afbService, IAFBAllianceService allianceService, IAFBTeamService afbTeamService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IAFBService = afbService;
            _IAFBAllianceService = allianceService;
            _IAFBTeamService = afbTeamService;
        }
        //
        // GET: /AmericanFootball/Log/

        public ActionResult Index()
        {
            return View();
        }


        //赛程日志查询
        public ActionResult Schedule(IEnumerable<ModifyRecord> records)
        {
            List<Models.ViewModel.AFB> oldSchedules = new List<Models.ViewModel.AFB>();
            List<Models.ViewModel.AFB> newSchedules = new List<Models.ViewModel.AFB>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                AFBSchedules old = _IAFBService.JsonDeserialize(p.OldData);
                AFBSchedules New = _IAFBService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldSchedules.Add(new Models.ViewModel.AFB
                    {
                        AllianceName = _IAFBAllianceService.GetDataById(old.AllianceID),
                        GameDate = old.GameDate,
                        GameTime = old.GameTime,
                        GameType = old.GameType,
                        TeamAName = _IAFBTeamService.GetDataById(old.TeamAID),
                        TeamBName = _IAFBTeamService.GetDataById(old.TeamBID),
                        GameStates = old.GameStates,
                        CtrlStates = old.CtrlStates,
                        CtrlAdmin = old.CtrlAdmin,
                        WebID = old.WebID,
                        TrackerText = old.TrackerText,
                        ShowJS = old.ShowJS,
                    });
                }
                if (New != null)
                {
                    newSchedules.Add(new Models.ViewModel.AFB
                    {
                        AllianceName = _IAFBAllianceService.GetDataById(New.AllianceID),
                        GameDate = New.GameDate,
                        GameTime = New.GameTime,
                        GameType = New.GameType,
                        TeamAName = _IAFBTeamService.GetDataById(New.TeamAID),
                        TeamBName = _IAFBTeamService.GetDataById(New.TeamBID),
                        GameStates = New.GameStates,
                        CtrlStates = New.CtrlStates,
                        CtrlAdmin = New.CtrlAdmin,
                        WebID = New.WebID,
                        TrackerText = New.TrackerText,
                        ShowJS = New.ShowJS,
                    });
                }
            });
            return View(Tuple.Create(oldSchedules, newSchedules, list[0].ActionStatus));
        }

        /// <summary>
        /// 队伍
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult Team(IEnumerable<ModifyRecord> records)
        {
            List<Models.ViewModel.AFBAllianceTeam> oldTeam = new List<Models.ViewModel.AFBAllianceTeam>();
            List<Models.ViewModel.AFBAllianceTeam> newTeam = new List<Models.ViewModel.AFBAllianceTeam>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                AFBTeam old = _IAFBTeamService.JsonDeserialize(p.OldData);
                AFBTeam New = _IAFBTeamService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldTeam.Add(new Models.ViewModel.AFBAllianceTeam
                    {
                        GameType = old.GameType,
                        TeamName = old.TeamName,
                        ShowName = old.ShowName,
                        WebName = old.WebName,
                        AllianceName = _IAFBAllianceService.GetDataById(old.AllianceID),
                        W = old.W,
                        L = old.L,
                        T = old.T
                    });
                }
                if (New != null)
                {
                    newTeam.Add(new Models.ViewModel.AFBAllianceTeam
                    {
                        GameType = New.GameType,
                        TeamName = New.TeamName,
                        ShowName = New.ShowName,
                        WebName = New.WebName,
                        AllianceName = _IAFBAllianceService.GetDataById(New.AllianceID),
                        W = New.W,
                        L = New.L,
                        T = New.T
                    });
                }
            });
            return View(Tuple.Create(oldTeam, newTeam, list[0].ActionStatus));
        }

        /// <summary>
        /// 联盟
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult Alliance(IEnumerable<ModifyRecord> records)
        {
            List<AFBAlliance> oldAlliance = new List<AFBAlliance>();
            List<AFBAlliance> newAlliance = new List<AFBAlliance>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                AFBAlliance old = _IAFBAllianceService.JsonDeserialize(p.OldData);
                AFBAlliance New = _IAFBAllianceService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldAlliance.Add(new AFBAlliance
                    {
                        AllianceID = old.AllianceID,
                        GameType = old.GameType,
                        Lever = old.Lever,
                        AllianceName = old.AllianceName,
                        LeverOther = old.LeverOther,
                        IsDeleted = old.IsDeleted,
                        AllianceUrl = old.AllianceUrl
                    });
                }
                if (New != null)
                {
                    newAlliance.Add(new AFBAlliance
                    {
                        AllianceID = New.AllianceID,
                        GameType = New.GameType,
                        Lever = New.Lever,
                        AllianceName = New.AllianceName,
                        LeverOther = New.LeverOther,
                        IsDeleted = New.IsDeleted,
                        AllianceUrl = New.AllianceUrl
                    });
                }
            });
            return View(Tuple.Create(oldAlliance, newAlliance, list[0].ActionStatus));
        }
    }
}
