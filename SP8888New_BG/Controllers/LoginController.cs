using Common;
using IServices;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SP8888New_BG.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        private ILoginService _LoginService;

        private IUser _IUser;

        public LoginController(ILoginService loginService, IUser IUser)
        {
            _LoginService = loginService;
            _IUser = IUser;
        }
        public ActionResult Index(bool? isload)
        {
            ViewBag.isload = (isload != null && isload.Value) ? true : false;
            return View();
        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult LogOn(LoginModel employee)
        {
            if (ModelState.IsValid)
            {
                Employee user = _LoginService.Login(new Employee { EmployeeName = employee.UserName, Password = employee.Password });
                if (user != null)
                {
                    if (!_LoginService.IpAccess_Check(user))
                    {
                        ViewBag.LoginFailed = "ip验证错误";
                        ViewBag.isload = false;
                        return View("Index");
                    }
                    user.Sid = AppData.CreateRandomCode(24);//登录成功，写入24为随机字符，用于比对是否重读登录
                    DateTime loginDate=DateTime.Now;
                    user.LoginDate = loginDate.Date;
                    user.LoginTime = loginDate.Subtract(user.LoginDate.Value);
                    _LoginService.Update(user);
                    _LoginService.Commit();
                    FormsAuthentication.SetAuthCookie(user.Rank + "," + user.EmployeeName + "," + user.Sid, true);
                    return RedirectToAction("Index", "SPBG", new { area = "SPBG" });
                }
                ViewBag.LoginFailed = "帳號或密碼錯誤";
            }
            ViewBag.isload = false;
            return View("Index");
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            //Session["userAccount"] = null;
            //Session["userRank"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
        /// <summary>
        /// 错误页
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}
