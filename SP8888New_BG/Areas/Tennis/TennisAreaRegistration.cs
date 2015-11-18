using System.Web.Mvc;

namespace SP8888New_BG.Areas.Tennis
{
    public class TennisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tennis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tennis_default",
                "Tennis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
