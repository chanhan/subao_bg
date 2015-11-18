using System.Web.Mvc;

namespace SP8888New_BG.Areas.Basketball
{
    public class BasketballAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Basketball";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Basketball_default",
                "Basketball/{controller}/{action}/{id}",
                new { action = "Index",controller="BKOS", id = UrlParameter.Optional }
            );
        }
    }
}
