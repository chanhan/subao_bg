using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Services;
using IServices;
using Models;
namespace SP8888New_BG.Controllers
{
    public class IndexController : Controller
    {

        //這是例子

        //Basketball basketball = new Basketball();
        //
        // GET: /Index/
        private ILoginService _LoginService;
        public IndexController(ILoginService loginService)
        {
            _LoginService = loginService;
        }
    
        public ActionResult Index()
        {
            //OSTeam team = new OSTeam
            //{
            //    AllianceID = 100,
            //    ShowName = "NBA",
            //    TeamName = "NBA"
            //};
        //    basketball.AddBasketBall(team);
            _LoginService.Login(new Employee { EmployeeName="a",Password="b"});
            return View();
        }

    }
}
