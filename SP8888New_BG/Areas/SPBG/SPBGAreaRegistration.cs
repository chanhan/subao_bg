using System.Web.Mvc;

namespace SP8888New_BG.Areas.SPBG
{
    public class SPBGAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SPBG";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SPBG_default",
                "SPBG/{controller}/{action}/{id}",
                new { action = "Index", controller="SPBG", id = UrlParameter.Optional }
            );
        }
    }
}
