using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Common;
using SP8888New_BG.Controllers;

namespace SP8888New_BG.Areas.Baseball.Controllers
{
    public class BBAllianceController : BaseController
    {
        public IBaseballAllianceService _IBaseballAllianceService;

        public BBAllianceController(IEmployeeService employeeService, IBaseballAllianceService IBaseballAlliance)
            : base(employeeService)
        {
            _IBaseballAllianceService = IBaseballAlliance;
        }
        /// <summary>
        /// 主頁顯示
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string gameType, string sMsg = "")
        {
            List<BaseballAlliance> ba = _IBaseballAllianceService.getAllianceList(gameType);
            var alliance = ba.Where(p => p.Lever == 1).Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceName.Replace(" ", "").Replace(":", "") }).ToList();
            alliance.Add(new SelectListItem { Text = "全部", Value = "", Selected = true });
            ViewBag.alliance = alliance;
            ViewBag.gameType = gameType.ToUpper();
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "聯盟管理" },
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
            ViewBag.msg = sMsg;
            return View(ba);
        }

        /// <summary>
        /// 新增聯盟主頁
        /// </summary>
        public ActionResult Create(string gameType, string sMsg = "")
        {
            //所屬大聯盟
            ViewBag.alliance1 = _IBaseballAllianceService.QueryByCondition(p => p.GameType == gameType && p.Lever == 1 && !p.IsDeleted).ToList().Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() });
            //所屬二聯盟
            ViewBag.alliance2 = _IBaseballAllianceService.QueryByCondition(p => p.GameType == gameType && p.Lever == 2 && !p.IsDeleted).ToList().Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString() });

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(gameType), "聯盟新增" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", gameType)
                },
                ButtonText = "回聯盟管理"
            };
            ViewBag.sActionName = "Create";
            ViewBag.sString = "新增";
            ViewBag.msg = sMsg;
            return View("AllianceEdit", new BaseballAlliance());
        }

        /// <summary>
        /// 新增请求
        /// </summary>
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            int c = 0;
            c = _IBaseballAllianceService.CreateAlliance(GetModel(collection));
            if (c > 0)
            {

                return RedirectToAction("Index", new { gameType = collection["GameType"], sMsg ="新增成功。"});
            }
            else
            {
                return RedirectToAction("Create", new { gameType = collection["GameType"], sMsg = c == -1 ? "存在重複的聯盟名稱！" : c == -2 ? "網址需加入http或https協議！" : "新增失敗！" });
            }
        }

        /// <summary>
        /// 修改聯盟主頁
        /// </summary>
        public ActionResult Edit(int allianceID, string sMsg = "")
        {
            BaseballAlliance ba = _IBaseballAllianceService.QueryById(allianceID);
            ba.LeverOther = ba.LeverOther ?? "";
            string[] leverOther1 = ba.LeverOther.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            //所屬大聯盟
            ViewBag.alliance1 = _IBaseballAllianceService.QueryByCondition(p => p.GameType == ba.GameType && p.Lever == 1 && !p.IsDeleted).ToList().Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString(), Selected = (ba.Lever != 1 && p.AllianceID.ToString() == leverOther1[0]) });
            //所屬二聯盟
            ViewBag.alliance2 = _IBaseballAllianceService.QueryByCondition(p => p.GameType == ba.GameType && p.Lever == 2 && !p.IsDeleted).ToList().Select(p => new SelectListItem { Text = p.AllianceName, Value = p.AllianceID.ToString(), Selected = (ba.Lever == 3 && p.AllianceID.ToString() == leverOther1[1]) });

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(ba.GameType), "聯盟修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", DateTime.Now.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", ba.GameType)
                },
                ButtonText = "回聯盟管理"
            };
            ViewBag.sActionName = "Edit";
            ViewBag.sString = "修改";
            ViewBag.msg = sMsg;
            return View("AllianceEdit", ba);
        }

        /// <summary>
        /// 修改请求
        /// </summary>
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            int c = 0;
            c = _IBaseballAllianceService.EditAlliance(GetModel(collection));
            if (c > 0)
            {
                return RedirectToAction("Index", new { gameType = collection["GameType"], sMsg = "修改成功。" });
            }
            else
            {
                return RedirectToAction("Edit", new { allianceID = collection["allianceID"], sMsg = c == -1 ? "存在重複的聯盟名稱！" : c == -2 ? "網址需加入http或https協議！" : "修改失敗！" });
            }
        }

        private BaseballAlliance GetModel(FormCollection collection)
        {
            string leverOther = string.Empty;
            int lever = 0;
            if (!int.TryParse(collection["Lever"], out lever))
            {
                return null;
            }
            if (lever == 2)
            {
                leverOther = "*" + collection["leverOther1" + lever] + "*";
            }
            else if (lever == 3)
            {
                leverOther = "*" + collection["leverOther1" + lever] + "*" + collection["leverOther2" + lever] + "*";
            }
            BaseballAlliance ba = new BaseballAlliance();
            ba.AllianceID = Convert.ToInt32(collection["AllianceID"]);
            ba.Lever = lever;
            ba.GameType = collection["GameType"].ToUpper();
            ba.AllianceName = collection["allianceName" + lever];
            ba.AllianceUrl = collection["allianceURL" + lever];
            ba.LeverOther = leverOther;
            ba.IsDeleted = false;

            return ba;
        }
    }
}
