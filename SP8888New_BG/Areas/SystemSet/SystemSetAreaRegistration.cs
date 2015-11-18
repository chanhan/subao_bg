using System.Web.Mvc;

namespace SP8888New_BG.Areas.SystemSet
{
    public class SystemSetAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemSet";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SystemSet_default",
                "SystemSet/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
