using System.Web.Mvc;

namespace SP8888New_BG.Areas.Baseball
{
    public class BaseballAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Baseball";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Baseball_default",
                "Baseball/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
