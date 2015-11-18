using Common;
using IServices;
using Models;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;


namespace SP8888New_BG.Areas.Foolball.Controllers
{
    public class FootballController : BaseController
    {
        //
        // GET: /Foolball/Foolball/

        private IFootballService _IFoolballService;
        private IScoreModifyRecordService _ISMRService;

        public FootballController(IFootballService foolbalService, IEmployeeService employeeService, IScoreModifyRecordService ISMRService)
        : base(employeeService)
        {
            _IFoolballService = foolbalService;
            _ISMRService = ISMRService;
        }

        //主页显示
        public ActionResult Index(DateTime date)
        {
            List<FootballSchedules> foolballs = _IFoolballService.GetFoolballSchedules(date);
            List<FollballAlliance> alliances = foolballs.Select(f => new FollballAlliance { AllianceName = f.AL, ShowName = f.AL }).Distinct(new Common.SBAllianceComparer()).ToList();
            ViewBag.ddlAllian = alliances.Select(c => new SelectListItem { Text = c.ShowName, Value = c.AllianceName });
            ViewBag.date = date.ToString("yyyy-MM-dd");
            if (foolballs == null || foolballs.Count == 0)
            {
                ViewBag.message = "暫無賽事！！";
            }
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "足球", "賽程資料" },
                HaveButton = false
            };
            return View(foolballs);
        }
        //分数修改页面
        public ActionResult ScoreEdit(int sId, string StrMessage)
        {
            ViewBag.message = StrMessage;
            FootballSchedules schedules = _IFoolballService.QueryById(sId);
            ViewBag.strTime = schedules.KO;
            if (schedules.KO.IndexOf('<') != -1)
            {
                ViewBag.strTime = "";
            }
            else if (schedules.KO.IndexOf(' ') != -1)
            {
                ViewBag.strDate = schedules.KO.Split(' ')[0].Replace('/', '-');
                ViewBag.strTime = schedules.KO.Split(' ')[1];
            }
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "足球", "分數修改" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                HaveButton = true,
                ButtonText = "回賽程頁",
                Action = "Index",
                Parameter = new List<Parameter>{
                    new Parameter("date", schedules.GameDate.ToString("yyyy-MM-dd")),
                }
            };
            return View(schedules);
        }

        //存储按钮
        [HttpPost]
        public ActionResult ScoreSave(FootballSchedules model, int modifyItem, int UP, string strTime)
        {
            string message = string.Empty;
            if (modifyItem == 0)
            {
                message = "* 儲存前先選擇修改項目";
            }
            else
            {
                //比赛时间只能修改时间部分
                if (model.KO.IndexOf(' ') != -1)
                {
                    model.KO = model.KO.Split(' ')[0] + " " + strTime;
                }
                else
                {
                    model.KO = strTime;
                }

                model.UP = GetUpString(UP);
                bool isCG = _IFoolballService.ScoreSave(model, modifyItem);
                message = isCG ? "* 修改成功！！" : " * 修改失敗！！";
            }
            return RedirectToAction("ScoreEdit", "Football", new { sId = model.ID, StrMessage = "+" + System.Web.HttpUtility.UrlEncode(message) });
        }

        //删除按钮
        public ActionResult ScoreDelete(int id)
        {
            _IFoolballService.ScoreDelete(id);
            return RedirectToAction("ScoreEdit", new { sId = id });
        }


        /// <summary>
        /// 分数修改记录
        /// </summary>
        /// <param name="GID"></param>
        /// <param name="GameType"></param>
        /// <returns></returns>
        public ActionResult ScoreModifyRecord(int GID, string GameType="SB")
        {
            FootballSchedules b = _IFoolballService.QueryById(GID);
            if (b == null)
            {
                return null;
            }
            List<ScoreModifyRecord> list = _ISMRService.QueryByCondition(p => p.ScheduleID == GID && p.GameType == GameType).ToList();
            ViewBag.sAlliance = b.AL;
            ViewBag.GameDate = b.KO;
            ViewBag.dw = b.NA + " vs. " + b.NB;

            List<SelectListItem> arr = list.Select(p => p.ModifyTime.ToString("yyyy-MM-dd")).Distinct().ToList().Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            arr.Insert(0, new SelectListItem { Text = "全部", Value = "", Selected = true });
            ViewBag.ddlDate = arr;

            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName(GameType), "比分修改紀錄" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = true,
                Parameter = new List<Models.ViewModel.Parameter> { 
                 new Parameter("date", b.GameDate.ToString("yyyy-MM-dd")),
                 new Parameter("gameType", GameType)
                },
                ButtonText = "返回賽程"
            };

            return View(list);
        }

        private string GetUpString(int up)
        {
            switch (up)
            {
                case 1:
                    return "完";
                case 2:
                    return "<font color='red'><b>改期</b></font>";
                case 3:
                    return "<font color='red'><b>腰斬</b></font>";
                case 4:
                    return "待定";
                case 5:
                    return "<font color='red'><b>取消</b></font>";
                case 6:
                    return "中斷";
                default:
                    return "";
            }
        }



        #region 角球隊伍資料
        private ConcurrentDictionary<string, string> TeamNameMap = new ConcurrentDictionary<string, string>();//集合
        private static string path = ConfigurationManager.AppSettings["XmlPath"];//xml路徑
        private double iPageSize = 15.0;
        /// <summary>
        /// 首页
        /// page 当前页 
        /// key  关键字
        /// </summary>
        /// <returns></returns>
        public ActionResult FoolballCorner(int pageIndex = 1, string key = "")
        {
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName("SB"), "角球隊伍資料 " },
                Controller = RouteData.Values["controller"].ToString(),
                Action = "Index",
                HaveButton = false
            };
            LoadXml();
            int iPageSizeTemp = Convert.ToInt32(iPageSize);
            IEnumerable<KeyValuePair<string, string>> list;
            if (string.IsNullOrWhiteSpace(key))
            {
                list = (from c in TeamNameMap select c).Skip(iPageSizeTemp * (pageIndex - 1)).Take(iPageSizeTemp);
                ViewBag.iCountPage = TeamNameMap.Count;
            }
            else
            {
                var temp = TeamNameMap.Where(p => p.Key.Contains(key) || p.Value.Contains(key));
                list = (from c in temp select c).Skip(iPageSizeTemp * (pageIndex - 1)).Take(iPageSizeTemp);
                ViewBag.iCountPage = temp.Count();
            }
            ViewBag.iCurrentPage = pageIndex;
            ViewBag.iPageSize = iPageSizeTemp;
            ViewBag.keyW = key;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "足球", "角球隊伍" },
                HaveButton = false
            };
            return View(list);
        }

        public ActionResult FoolballCornerUpdate(CornerTeamName ctn, string operation = "")
        {
            ViewBag.IsUpdate = operation.ToLower() == "update";
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "足球", "角球隊伍" },
                HaveButton = false
            };
            return View(ctn);
        }

        [HttpPost]
        public ActionResult EditJQ(CornerTeamName newCtn, string HdlSpName, string HdlTsName)
        {
            if (newCtn != null &&
                !string.IsNullOrEmpty(newCtn.SpName) &&
                !string.IsNullOrEmpty(newCtn.TsName) &&
                !string.IsNullOrEmpty(HdlSpName) &&
                !string.IsNullOrEmpty(HdlTsName)
               )
            {
                string txtSpName = newCtn.SpName;
                string txtTsName = newCtn.TsName;
                string oldSpName = HdlSpName;
                string oldTsName = HdlTsName;
                XElement doc = XElement.Load(path);
                try
                {
                    var item = (from c in doc.Descendants("data")
                                where c.Attribute("TsName").Value == oldTsName &&
                                     c.Attribute("SpName").Value == oldSpName
                                select c).FirstOrDefault();
                    item.SetAttributeValue("TsName", txtTsName);
                    item.SetAttributeValue("SpName", txtSpName);
                    doc.Save(path);
                    return Json("ok");
                }
                catch
                {
                    return Json("error");
                }
            }
            return Json("0");

        }

        [HttpPost]
        public ActionResult CreateJQ(CornerTeamName newCtn)
        {
            if (!string.IsNullOrEmpty(newCtn.SpName) &&
                     !string.IsNullOrEmpty(newCtn.TsName)
                     )
            {
                string txtSpName = newCtn.SpName;
                string txtTsName = newCtn.TsName;
                try
                {
                    XElement doc = XElement.Load(path);
                    string result = "<data SpName=\"" + txtSpName + "\"  TsName=\"" + txtTsName + "\"/>";
                    XElement xElement = XElement.Parse(result);
                    doc.Add(xElement);
                    //保存到XML文件中  
                    doc.Save(path);
                    return Json("ok");
                }
                catch(Exception e)
                {
                    return Json("error:"+e.Message+e.StackTrace);
                }
            }
            return Json("0");

        }

        [HttpPost]
        public ActionResult DeleteJQ(CornerTeamName ctn)
        {
            XElement doc = XElement.Load(path);
            try
            {
                var item = (from c in doc.Descendants("data")
                            where c.Attribute("TsName").Value == ctn.TsName &&
                                  c.Attribute("SpName").Value == ctn.SpName
                            select c).FirstOrDefault();
                item.Remove();
                doc.Save(path);
                return Json("ok");
            }
            catch
            {
                return Json("error:error");
            }
        }

        /// <summary>
        /// 加載xml
        /// </summary>
        private void LoadXml()
        {
            TeamNameMap.Clear();
            XElement doc = XElement.Load(path);
            foreach (var item in doc.Descendants("data"))
            {
                CornerTeamName cornerModel = new CornerTeamName();
                cornerModel.TsName = item.Attribute("TsName").Value;
                cornerModel.SpName = item.Attribute("SpName").Value;
                if (!TeamNameMap.ContainsKey(cornerModel.TsName))
                {
                    TeamNameMap.AddOrUpdate(cornerModel.SpName, cornerModel.TsName, (m, n) => cornerModel.TsName);
                }
            }
        }

        #endregion 角球隊伍資料
    }
}