using System.Web.Mvc;

namespace SP8888New_BG.Areas.AmericanFootball
{
    public class AmericanFootballAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AmericanFootball";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AmericanFootball_default",
                "AmericanFootball/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
