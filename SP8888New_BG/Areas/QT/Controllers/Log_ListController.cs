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

namespace SP8888New_BG.Areas.QT.Controllers
{
    public class Log_ListController : BaseController
    {
        private IOperationalRecordService _IOperationalRecordService;
        public Log_ListController(IOperationalRecordService OperationalRecordService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IOperationalRecordService = OperationalRecordService;
        }
        //
        // GET: /QT/Log_List/
        public ActionResult Index(DateTime date, int actionStatus, int itemCategory, string content, bool isCheckDate)
        {
            DateTime dateStart = date.Date;
            DateTime dateEnd = dateStart.AddDays(1);
            List<ModifyRecord> qr = _IOperationalRecordService.QueryByCondition(p => (isCheckDate ? (p.ChangeTime >= dateStart && p.ChangeTime < dateEnd) : true) && (actionStatus == 0 ? true : p.ActionStatus == actionStatus) && (itemCategory == 0 ? true : p.ItemCategory == itemCategory) && (string.IsNullOrEmpty(content) ? true : (p.OldData.Contains(content) || p.NewData.Contains(content)))).OrderByDescending(p => p.ChangeTime).ToList();
            ViewBag.date = date.ToString("yyyy-MM-dd");
            ViewBag.checkDate = isCheckDate;
            ViewBag.action = actionStatus;
            ViewBag.item = itemCategory;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "菜單", "操作記錄" },
                HaveButton = false
            };
            return View(qr);
        }
    }
}
