using System.Web.Mvc;

namespace SP8888New_BG.Areas.Foolball
{
    public class FootballAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Football";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Football_default",
                "Football/{controller}/{action}/{id}",
                new { controller = "Football", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}