using IServices;
using Models;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.Baseball.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /Baseball/Log/
        private IBaseballSchedulesService _IBaseballSchedulesService;
        private IBaseballAllianceService _IBaseballAllianceService;
        private IBaseballTeamService _IBaseballTeamService;

        public LogController(IBaseballSchedulesService baseballscheduleservice, IBaseballAllianceService baseballallianceservice, IBaseballTeamService baseballteamservice, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IBaseballSchedulesService = baseballscheduleservice;
            _IBaseballAllianceService = baseballallianceservice;
            _IBaseballTeamService = baseballteamservice;
        }
        public ActionResult Index()
        {
            return View();
        }
        //赛程日志查询
        public ActionResult Schedule(IEnumerable<ModifyRecord> records)
        {
            List<Models.ViewModel.Baseball> oldSchedules = new List<Models.ViewModel.Baseball>();
            List<Models.ViewModel.Baseball> newSchedules = new List<Models.ViewModel.Baseball>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BaseballSchedules old = _IBaseballSchedulesService.JsonDeserialize(p.OldData);
                BaseballSchedules New = _IBaseballSchedulesService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldSchedules.Add(new Models.ViewModel.Baseball
                    {
                        Alliance = _IBaseballAllianceService.GetDataById(old.AllianceID),
                        GameDate = old.GameDate,
                        GameTime = old.GameTime,
                        GameType = old.GameType,
                        TeamA = _IBaseballTeamService.GetDataById(old.TeamAID).ShowName,
                        TeamB = _IBaseballTeamService.GetDataById(old.TeamBID).ShowName,
                        GameStates = old.GameStates,
                        CtrlStates = old.CtrlStates,
                        CtrlAdmin = old.CtrlAdmin,
                        WebID = old.WebID,
                        TrackerText = old.TrackerText,
                        ShowJS = old.ShowJS,
                        IsReschedule = old.IsReschedule,
                    });
                }
                if (New != null)
                {
                    newSchedules.Add(new Models.ViewModel.Baseball
                    {
                        Alliance = _IBaseballAllianceService.GetDataById(New.AllianceID),
                        GameDate = New.GameDate,
                        GameTime = New.GameTime,
                        GameType = New.GameType,
                        TeamA = _IBaseballTeamService.GetDataById(New.TeamAID).ShowName,
                        TeamB = _IBaseballTeamService.GetDataById(New.TeamBID).ShowName,
                        GameStates = New.GameStates,
                        CtrlStates = New.CtrlStates,
                        CtrlAdmin = New.CtrlAdmin,
                        WebID = New.WebID,
                        TrackerText = New.TrackerText,
                        ShowJS = New.ShowJS,
                        IsReschedule = New.IsReschedule,
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
        public ActionResult Team (IEnumerable<ModifyRecord> records)
        {
            List<BaseballTeam> oldTeam = new List<BaseballTeam>();
            List<BaseballTeam> newTeam = new List<BaseballTeam>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BaseballTeam old = _IBaseballTeamService.JsonDeserialize(p.OldData);
                BaseballTeam New = _IBaseballTeamService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldTeam.Add(new BaseballTeam
                    {
                        TeamID = old.TeamID,
                        GameType = old.GameType,
                        TeamName = old.TeamName,
                        ShowName = old.ShowName,
                        WebName = old.WebName,
                        AllianceID = old.AllianceID,
                        AllianceName = _IBaseballAllianceService.GetDataById(old.AllianceID),
                        W = old.W,
                        L = old.L,
                        T = old.T,
                        IsDeleted = old.IsDeleted,
                        SourceID = old.SourceID
                    });
                }
                if (New != null)
                {
                    newTeam.Add(new BaseballTeam
                    {
                        TeamID = New.TeamID,
                        GameType = New.GameType,
                        TeamName = New.TeamName,
                        ShowName = New.ShowName,
                        WebName = New.WebName,
                        AllianceID = New.AllianceID,
                        AllianceName = _IBaseballAllianceService.GetDataById(New.AllianceID),
                        W = New.W,
                        L = New.L,
                        T = New.T,
                        IsDeleted = New.IsDeleted,
                        SourceID = New.SourceID
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
            List<BaseballAlliance> oldAlliance = new List<BaseballAlliance>();
            List<BaseballAlliance> newAlliance = new List<BaseballAlliance>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BaseballAlliance old = _IBaseballAllianceService.JsonDeserialize(p.OldData);
                BaseballAlliance New = _IBaseballAllianceService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldAlliance.Add(new BaseballAlliance
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
                    newAlliance.Add(new BaseballAlliance
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
