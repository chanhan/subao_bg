using IServices;
using Models;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.IceHockey.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /IceHockey/Log/

        private IIceHockeySchedulesService _IIceHockeySchedulesService;
        private IIceHockeyAllianceService _IIceHockeyAllianceService;
        private IIceHockeyTeamService _IIceHockeyTeamService;

        public LogController(IIceHockeySchedulesService ihService, IIceHockeyAllianceService ia, IIceHockeyTeamService it, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IIceHockeySchedulesService = ihService;
            _IIceHockeyAllianceService = ia;
            _IIceHockeyTeamService = it;
        }

        public ActionResult Index()
        {
            return View();
        }

        //赛程日志查询
        public ActionResult Schedule(IEnumerable<ModifyRecord> records)
        {
            List<Models.ViewModel.IceHockey> oldSchedules = new List<Models.ViewModel.IceHockey>();
            List<Models.ViewModel.IceHockey> newSchedules = new List<Models.ViewModel.IceHockey>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                IceHockeySchedules old = _IIceHockeySchedulesService.JsonDeserialize(p.OldData);
                IceHockeySchedules New = _IIceHockeySchedulesService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldSchedules.Add(new Models.ViewModel.IceHockey
                    {
                        Alliance = _IIceHockeyAllianceService.GetDataById(old.AllianceID),
                        GameDate = old.GameDate,
                        GameTime = old.GameTime,
                        GameType = old.GameType,
                        TeamA = _IIceHockeyTeamService.GetDataById(old.TeamAID),
                        TeamB = _IIceHockeyTeamService.GetDataById(old.TeamBID),
                        GameStates = old.GameStates,
                        CtrlStates = old.CtrlStates,
                        CtrlAdmin = old.CtrlAdmin,
                        WebID = old.WebID,
                        TrackerText = old.TrackerText,
                        ShowJS = old.ShowJS,
                        Display = old.Display,
                    });
                }
                if (New != null)
                {
                    newSchedules.Add(new Models.ViewModel.IceHockey
                    {
                        Alliance = _IIceHockeyAllianceService.GetDataById(New.AllianceID),
                        GameDate = New.GameDate,
                        GameTime = New.GameTime,
                        GameType = New.GameType,
                        TeamA = _IIceHockeyTeamService.GetDataById(New.TeamAID),
                        TeamB = _IIceHockeyTeamService.GetDataById(New.TeamBID),
                        GameStates = New.GameStates,
                        CtrlStates = New.CtrlStates,
                        CtrlAdmin = New.CtrlAdmin,
                        WebID = "",
                        TrackerText = New.TrackerText,
                        ShowJS = New.ShowJS,
                        Display = New.Display
                    });
                }
            });
            return View(Tuple.Create(oldSchedules, newSchedules, list[0].ActionStatus));
        }


        /// <summary>
        /// 队伍日志查询
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult Team(IEnumerable<ModifyRecord> records)
        {
            List<IceHockeyTeam> oldTeam = new List<IceHockeyTeam>();
            List<IceHockeyTeam> newTeam = new List<IceHockeyTeam>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                IceHockeyTeam old = _IIceHockeyTeamService.JsonDeserialize(p.OldData);
                IceHockeyTeam New = _IIceHockeyTeamService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldTeam.Add(new IceHockeyTeam
                    {
                        TeamID = old.TeamID,
                        GameType = old.GameType,
                        TeamName = old.TeamName,
                        ShowName = old.ShowName,
                        WebName = old.WebName,
                        AllianceID = old.AllianceID,
                        AllianceName = _IIceHockeyAllianceService.GetDataById(old.AllianceID),
                        W = old.W,
                        L = old.L,
                        T = old.T,
                        Display = old.Display
                    });
                }
                if (New != null)
                {
                    newTeam.Add(new IceHockeyTeam
                    {
                        TeamID = New.TeamID,
                        GameType = New.GameType,
                        TeamName = New.TeamName,
                        ShowName = New.ShowName,
                        WebName = New.WebName,
                        AllianceID = New.AllianceID,
                        AllianceName = _IIceHockeyAllianceService.GetDataById(New.AllianceID),
                        W = New.W,
                        L = New.L,
                        T = New.T,
                        Display = New.Display
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
            List<IceHockeyAlliance> oldAlliance = new List<IceHockeyAlliance>();
            List<IceHockeyAlliance> newAlliance = new List<IceHockeyAlliance>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                IceHockeyAlliance old = _IIceHockeyAllianceService.JsonDeserialize(p.OldData);
                IceHockeyAlliance New = _IIceHockeyAllianceService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldAlliance.Add(new IceHockeyAlliance
                    {
                        AllianceID = old.AllianceID,
                        GameType = old.GameType,
                        Lever = old.Lever,
                        AllianceName = old.AllianceName,
                        LeverOther = old.LeverOther,
                        AllianceUrl = old.AllianceUrl,
                        ShowName = old.ShowName,
                        Display = old.Display
                    });
                }
                if (New != null)
                {
                    newAlliance.Add(new IceHockeyAlliance
                    {
                        AllianceID = New.AllianceID,
                        GameType = New.GameType,
                        Lever = New.Lever,
                        AllianceName = New.AllianceName,
                        LeverOther = New.LeverOther,
                        AllianceUrl = New.AllianceUrl,
                        ShowName = New.ShowName,
                        Display = New.Display
                    });
                }
            });
            return View(Tuple.Create(oldAlliance, newAlliance, list[0].ActionStatus));
        }

    }
}
