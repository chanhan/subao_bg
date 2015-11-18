using IServices;
using Models;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.Tennis.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /Tennis/Log/


        private ITennisService _ITennisService;
        private ITennisAllianceService _ITennisAllianceService;
        private INameControlService _INameControlService;
        public LogController(ITennisService tennisService, ITennisAllianceService tennisAllianceService,INameControlService namecontrolservice, IEmployeeService employeeService)
            : base(employeeService)
        {
            _ITennisService = tennisService;
            _ITennisAllianceService = tennisAllianceService;
            _INameControlService = namecontrolservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        //赛程日志查询
        public ActionResult Schedule(IEnumerable<ModifyRecord> records)
        {
            List<Models.ViewModel.Tennis> oldSchedules = new List<Models.ViewModel.Tennis>();
            List<Models.ViewModel.Tennis> newSchedules = new List<Models.ViewModel.Tennis>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                TennisSchedules old = _ITennisService.JsonDeserialize(p.OldData);
                TennisSchedules New = _ITennisService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldSchedules.Add(new Models.ViewModel.Tennis
                    {
                        AllianceName = _ITennisAllianceService.GetDataById(old.AllianceID).AllianceName,
                        GameDate = old.GameDate,
                        GameTime = old.GameTime,
                        TeamAName =_INameControlService.GetDataById(old.TeamAID),
                        TeamBName = _INameControlService.GetDataById(old.TeamBID),
                        GameStates = old.GameStates,
                        AllianceShowName =_ITennisAllianceService.GetDataById(old.AllianceID).ShowName,
                        IsDeleted = old.IsDeleted
                    });
                }
                if (New != null)
                {
                    newSchedules.Add(new Models.ViewModel.Tennis
                    {
                        AllianceName = _ITennisAllianceService.GetDataById(New.AllianceID).AllianceName,
                        GameDate = New.GameDate,
                        GameTime = New.GameTime,
                        TeamAName = _INameControlService.GetDataById(New.TeamAID),
                        TeamBName = _INameControlService.GetDataById(New.TeamBID),
                        GameStates = New.GameStates,
                        AllianceShowName = _ITennisAllianceService.GetDataById(New.AllianceID).ShowName,
                        IsDeleted = New.IsDeleted
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
            List<TennisAlliance> oldAlliance = new List<TennisAlliance>();
            List<TennisAlliance> newAlliance = new List<TennisAlliance>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                TennisAlliance old = _ITennisAllianceService.JsonDeserialize(p.OldData);
                TennisAlliance New = _ITennisAllianceService.JsonDeserialize(p.NewData);
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

    }
}
