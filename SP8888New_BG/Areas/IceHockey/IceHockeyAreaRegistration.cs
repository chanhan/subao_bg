using System.Web.Mvc;

namespace SP8888New_BG.Areas.IceHockey
{
    public class IceHockeyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IceHockey";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IceHockey_default",
                "IceHockey/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
