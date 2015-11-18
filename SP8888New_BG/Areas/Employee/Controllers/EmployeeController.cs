using Common;
using IServices;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.Employee.Controllers
{
    public class EmployeeController : BaseController
    {
        private IEmployeeService _IEmployeeService;
        private IUser _IUser;
        public EmployeeController(IEmployeeService employeeservice,IUser user):base(employeeservice)
        {
            _IEmployeeService = employeeservice;
            _IUser = user;
        }
        //
        // GET: /Employee/Employee/

        /// <summary>
        /// 根据等级获取所有员工信息
        /// </summary>
        /// <param name="rank">等级参数</param>
        /// <returns></returns>
        public ActionResult Index(string rank)
        {
            List<Models.Employee> emp = _IEmployeeService.GetEmployeeAllByRank(rank);
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName("AcMa"), AppData.GetEmployeeRankName(rank) },
                HaveButton = false
            };
            ViewBag.tuple = Tuple.Create(_IUser.UserRank, rank);
            return View(emp);
        }

        /// <summary>
        /// 添加修改视图
        /// </summary>
        /// <param name="employeename">如果是添加：当前查看的页面的员工的等级，如果是修改：员工的帐号</param>
        /// <returns></returns>
        public ActionResult EditEmployee(string parameter, string edit)
        {
            string employeeRankName = "";//当前对哪个等级下的员工进行操作
            string action = "";//动作描述
            Models.Employee emp = new Models.Employee();
            if (parameter == null && edit == null)//修改密码
            {
                action = "修改帳號";
                ViewBag.action = "修改";
                ViewData["Pwd"] = "Pwd";
                emp = _IEmployeeService.GetEmployeeByName(_IUser.UserName);
            }
            else
            {
                //修改视图返回一条员工信息
                if (edit == "update")
                {
                    emp = _IEmployeeService.GetEmployeeByName(parameter);
                    employeeRankName = AppData.GetEmployeeRankName(emp.Rank);
                    action = "修改帳號";
                    ViewBag.action = "修改";
                }
                else
                {
                    employeeRankName = AppData.GetEmployeeRankName(parameter);
                    action = "新增帳號";
                    ViewBag.action = "添加";
                    ViewBag.Rank = parameter;
                }
            }
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { AppData.GetGameTypeName("AcMa"), employeeRankName, action },
                HaveButton = false
            };
            return View(emp);
        }

        [HttpPost]
        public ActionResult EditEmployee(string NewName, string Pwd, string Rank, string OldName, string edit)
        {
            Models.Employee emp = new Models.Employee(){EmployeeName=NewName,Password = Pwd,Rank=Rank};
            return Json(_IEmployeeService.UpdateEmployee(emp, OldName, edit));
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="employeename">帐号</param>
        /// <returns></returns>
        public ActionResult delete(string employeename)
        {
            return Json(_IEmployeeService.Delete(employeename));
        }
    }
}
