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
    public class LogController : BaseController
    {
        //
        // GET: /Basketball/Log/
        private IBasketballService _IBasketballService;
        private IBasketballAllianceService _IBasketballAllianceService;
        private IBasketballTeamService _IBasketballTeamService;
        public ActionResult Index()
        {
            return View();
        }
        public LogController(IBasketballService basketballService, IBasketballAllianceService basketballianceservice, IBasketballTeamService basketballteamservice, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IBasketballService = basketballService;
            _IBasketballAllianceService = basketballianceservice;
            _IBasketballTeamService = basketballteamservice;
        }
        //赛程日志查询
        public ActionResult Schedule(IEnumerable<ModifyRecord> records)
        {
            List<BasketBall> oldSchedules = new List<BasketBall>();
            List<BasketBall> newSchedules = new List<BasketBall>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BasketballSchedules old = _IBasketballService.JsonDeserialize(p.OldData);
                BasketballSchedules New = _IBasketballService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldSchedules.Add(new BasketBall
                    {
                        AllianceName = _IBasketballAllianceService.GetDataById(old.AllianceID),
                        GameDate = old.GameDate,
                        GameTime = old.GameTime,
                        GameType = old.GameType,
                        TeamAName = _IBasketballTeamService.GetDataById(old.TeamAID),
                        TeamBName = _IBasketballTeamService.GetDataById(old.TeamBID),
                        GameStates = old.GameStates,
                        CtrlStates = old.CtrlStates,
                        CtrlAdmin = old.CtrlAdmin,
                        WebID = old.WebID,
                        TrackerText = old.TrackerText,
                        ShowJS = old.ShowJS
                    });
                }
                if (New != null)
                {
                    newSchedules.Add(new BasketBall {
                        AllianceName = _IBasketballAllianceService.GetDataById(New.AllianceID),
                        GameDate = New.GameDate,
                        GameTime = New.GameTime,
                        GameType = New.GameType,
                        TeamAName = _IBasketballTeamService.GetDataById(New.TeamAID),
                        TeamBName = _IBasketballTeamService.GetDataById(New.TeamBID),
                        GameStates = New.GameStates,
                        CtrlStates = New.CtrlStates,
                        CtrlAdmin = New.CtrlAdmin,
                        WebID = New.WebID,
                        TrackerText = New.TrackerText,
                        ShowJS = New.ShowJS
                    });
                }
            });
            return View(Tuple.Create(oldSchedules,newSchedules,list[0].ActionStatus));
        }

        /// <summary>
        /// 联盟
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult Alliance(IEnumerable<ModifyRecord> records)
        {
            List<BasketballAlliance> oldAlliance = new List<BasketballAlliance>();
            List<BasketballAlliance> newAlliance = new List<BasketballAlliance>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BasketballAlliance old = _IBasketballAllianceService.JsonDeserialize(p.OldData);
                BasketballAlliance New = _IBasketballAllianceService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldAlliance.Add(new BasketballAlliance
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
                    newAlliance.Add(new BasketballAlliance
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

        /// <summary>
        /// 队伍
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult Team(IEnumerable<ModifyRecord> records)
        {
            List<BasketballTeam> oldTeam = new List<BasketballTeam>();
            List<BasketballTeam> newTeam = new List<BasketballTeam>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                BasketballTeam old = _IBasketballTeamService.JsonDeserialize(p.OldData);
                BasketballTeam New = _IBasketballTeamService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldTeam.Add(new BasketballTeam
                    {
                        TeamID = old.TeamID,
                        GameType = old.GameType,
                        TeamName = old.TeamName,
                        ShowName = old.ShowName,
                        AllianceID = old.AllianceID,
                        AllianceName = _IBasketballAllianceService.GetDataById(old.AllianceID),
                        W = old.W,
                        L = old.L,
                        T = old.T,
                        IsDeleted = old.IsDeleted,
                        SourceID = old.SourceID,
                        WebName = old.WebName
                    });
                }
                if (New != null)
                {
                    newTeam.Add(new BasketballTeam
                    {
                        TeamID = New.TeamID,
                        GameType = New.GameType,
                        TeamName = New.TeamName,
                        ShowName = New.ShowName,
                        AllianceID = New.AllianceID,
                        AllianceName = _IBasketballAllianceService.GetDataById(New.AllianceID),
                        W = New.W,
                        L = New.L,
                        T = New.T,
                        IsDeleted = New.IsDeleted,
                        SourceID = New.SourceID,
                        WebName = New.WebName

                    });
                }
            });
            return View(Tuple.Create(oldTeam, newTeam, list[0].ActionStatus));
        }
    }
}
