﻿using Common;
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
    public class BBTeamController : BaseController
    {
        private IBaseballTeamService _IBaseballTeamService;
        private IBaseballAllianceService _IBaseballAllianceService;
        private ISourceTypeService _ISourceTypeService;

        public BBTeamController(IEmployeeService employeeService, IBaseballTeamService its, IBaseballAllianceService ias, ISourceTypeService ists)
            : base(employeeService)
        {
            _IBaseballTeamService = its;
            _IBaseballAllianceService = ias;
            _ISourceTypeService = ists;
        }

        public ActionResult Index(string gameType)
        {
            List<BaseballTeam> list = _IBaseballTeamService.getTeamList(gameType);

            //聯盟下拉框
            var alliance = _IBaseballAllianceService.QueryByCondition(p => p.GameType == gameType && !p.IsDeleted).ToList();
            List<SelectListItem> items = alliance.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() }).ToList();
            items.Insert(0, new SelectListItem { Text = "全部", Value = "", Selected = true });
            ViewBag.alliance = items;

            //資料來源下拉框
            var sourece = _ISourceTypeService.GetBBSourceType(gameType).Select(p => new SelectListItem { Text = p.GameSource, Value = p.SourceID }).ToList();
            sourece.Insert(0, new SelectListItem { Value = "", Text = "全部", Selected = true });
            ViewBag.sourece = sourece;

            ViewBag.gameType = gameType.ToUpper();

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "隊伍管理" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = "BBSchedules",
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", gameType)
                },
                ButtonText = "返回賽程"
            };
            return View(list);
        }


        /// <summary>
        /// 新增隊伍主頁
        /// </summary>
        public ActionResult Create(string gameType, string sMsg = "")
        {
            //聯盟下拉框
            var alliance = _IBaseballAllianceService.QueryByCondition(p => p.GameType == gameType && !p.IsDeleted).ToList();
            List<SelectListItem> items = alliance.Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() }).ToList();
            ViewBag.alliance = items;

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "隊伍新增" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", gameType)
                },
                ButtonText = "回隊伍管理"
            };
            ViewBag.sActionName = "Create";
            ViewBag.sString = "新增";
            ViewBag.msg = sMsg;
            return View("TeamEdit", new BaseballTeam());
        }

        /// <summary>
        /// 新增請求
        /// </summary>
        [HttpPost]
        public ActionResult Create(BaseballTeam bt)
        {
            if (bt.WebName != null)
            {
                bt.WebName = bt.WebName.Replace("\r\n", ",");
            }
            int c = _IBaseballTeamService.CreateTeam(bt);
            if (c > 0)
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index", new { gameType = bt.GameType });
            }
            else
            {
                return RedirectToAction("Create", new { gameType = bt.GameType, sMsg = c == -1 ? "存在重複的隊伍名稱！" : "新增失敗！" });
            }
        }

        /// <summary>
        /// 修改主頁
        /// </summary>
        public ActionResult Edit(int teamID, string sMsg = "")
        {
            BaseballTeam bt = _IBaseballTeamService.QueryById(teamID);
            //聯盟下拉框
            var alliance = _IBaseballAllianceService.QueryByCondition(p => p.GameType == bt.GameType && !p.IsDeleted).ToList();
            List<SelectListItem> items = alliance.Select(p => new SelectListItem { Selected = p.AllianceID == bt.AllianceID, Text = p.AllianceName, Value = p.AllianceID.ToString() }).ToList();
            ViewBag.alliance = items;

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(bt.GameType), "隊伍修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", bt.GameType)
                },
                ButtonText = "回隊伍管理"
            };
            ViewBag.sActionName = "Edit";
            ViewBag.sString = "修改";
            ViewBag.msg = sMsg;
            return View("TeamEdit", bt);
        }

        //
        // POST: /Baseball/BBTeam/Edit/5

        [HttpPost]
        public ActionResult Edit(BaseballTeam bt)
        {
            if (bt.WebName != null)
            {
                bt.WebName = bt.WebName.Replace("\r\n", ",");
            }
            int c = _IBaseballTeamService.EditTeam(bt);
            if (c > 0)
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index", new { gameType = bt.GameType });
            }
            else
            {
                return RedirectToAction("Edit", new { teamID = bt.TeamID, sMsg = c == -1 ? "存在重複的隊伍名稱！" : "修改失敗！" });
            }
        }

        //
        // POST: /Baseball/BBTeam/Delete/5

        [HttpPost]
        public ActionResult Delete(int teamID)
        {
            int c = _IBaseballTeamService.DeleteTeam(teamID);
            if (c > 0)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
    }
}
