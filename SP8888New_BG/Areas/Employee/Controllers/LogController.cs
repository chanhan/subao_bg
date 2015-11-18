using IServices;
using Models;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.Employee.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /Employee/Log/

        private IEmployeeService _IEmployeeService;

        public LogController(IEmployeeService employeeservice)
            : base(employeeservice)
        {
            _IEmployeeService = employeeservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        //员工日志
        public ActionResult Account(IEnumerable<ModifyRecord> records)
        {
            List<Models.Employee> oldAccount = new List<Models.Employee>();
            List<Models.Employee> newAccount = new List<Models.Employee>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                Models.Employee old = _IEmployeeService.JsonDeserialize(p.OldData);
                Models.Employee New = _IEmployeeService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldAccount.Add(old);
                }
                if (New != null)
                {
                    newAccount.Add(New);
                }
            });
            return View(Tuple.Create(oldAccount, newAccount, list[0].ActionStatus));
        }
    }
}
