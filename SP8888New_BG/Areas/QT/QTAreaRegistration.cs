using System.Web.Mvc;

namespace SP8888New_BG.Areas.QT
{
    public class QTAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "QT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "QT_default",
                "QT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
