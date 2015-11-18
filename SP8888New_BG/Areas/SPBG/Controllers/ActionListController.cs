using IServices;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SPBG.Controllers
{
    public class ActionListController : BaseController
    {
        //
        // GET: /SchedulesActionList/ActionList/
        private IBaseballSchedulesService _IBaseballSchedulesService;
        private IBasketballService _IBasketballService;
        private IIceHockeySchedulesService _IIceHockeySchedulesService;
        private IAFBService _IAFBService;
        private IActionListService _IActionListService;
        public ActionListController(IBaseballSchedulesService baseballSchedulesService, IBasketballService basketballService, IIceHockeySchedulesService iceHockeySchedulesService, IAFBService aFBService, IEmployeeService employeeService, IActionListService actionlistservice)
            : base(employeeService)
        {
            _IBaseballSchedulesService = baseballSchedulesService;
            _IBasketballService = basketballService;
            _IIceHockeySchedulesService = iceHockeySchedulesService;
            _IAFBService = aFBService;
            _IActionListService = actionlistservice;
        }
        public ActionResult Index(string gameType, DateTime date)
        {
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "操盤列表", "" },
                Area = RouteData.DataTokens["area"].ToString(),
                Controller = RouteData.Values["controller"].ToString(),
                Action = RouteData.Values["action"].ToString(),
                HaveButton = false
            };
            Models.ViewModel.ActionList ac = new Models.ViewModel.ActionList();
            ac.afb = _IActionListService.GetAFB(date, gameType);
            ac.baseball = _IActionListService.Getbaseball(date, gameType);
            ac.basketBall = _IActionListService.GetBasketball(date, gameType);
            ac.iceHockey = _IActionListService.GetIceHockey(date, gameType);
            ViewBag.tuple = Tuple.Create(date.AddDays(-1).ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd"), gameType);
            return View(ac);
        }

    }

}
