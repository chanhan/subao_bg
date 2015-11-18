using Common;
using IServices;
using Services;
using SP8888New_BG.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SP8888New_BG.Controllers
{
    public class BaseController : Controller
    {
        private IUser _IUser;
        private IEmployeeService _IEmployeeService;
        public BaseController(IEmployeeService employeeService)
        {
            _IEmployeeService = employeeService;
            _IUser = _IEmployeeService.User;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!string.IsNullOrEmpty(_IUser.UserName) && !string.IsNullOrEmpty(_IUser.Sid))
            {
                if (!_IEmployeeService.QueryByCondition(p => p.Sid == _IUser.Sid).Any())
                {
                    string path = Server.MapPath(WebConfigurationManager.AppSettings["logPath"]);
                    AppData.WriteLog(path, string.Format("{0}--{1}--{2}--{3}--{4}--{5}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), _IUser.UserName, _IUser.Sid, RouteData.DataTokens["area"].ToString(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString()));
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Login", new { area = "", isload = true }));
                }
            }
            else
            {
                filterContext.Result = new RedirectResult(Url.Action("Index", "Login", new { area = "" }));
            }
        }
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;
        //    string errMsg = filterContext.Exception.StackTrace;
        //    // 写错误日志,导向错误页
        //    filterContext.Result = new RedirectResult(Url.Action("Error", "Login", new { area = "" }));
        //}
    }
}