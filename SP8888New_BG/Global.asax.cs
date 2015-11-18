using Autofac;
using Autofac.Integration.Mvc;
using Common;
using IServices;
using Services.Infrastructure;
using SP8888New_BG.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SP8888New_BG
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.Load("Services"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
            builder.RegisterType<User>().As<IUser>();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();

            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError().GetBaseException();
            StringBuilder str = new StringBuilder();
            str.Append("\r\n" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            string ip = "";
            if (Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                ip = Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            }
            else
            {
                ip = Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            str.Append("\r\n\tIp:" + ip);
            str.Append("\r\n\t瀏覽器:" + Request.Browser.Browser.ToString());
            str.Append("\r\n\t瀏覽器版本:" + Request.Browser.MajorVersion.ToString());
            str.Append("\r\n\t操作系統:" + Request.Browser.Platform.ToString());
            str.Append("\r\n\tURL：" + Request.Url.ToString());
            str.Append("\r\n\t錯誤信息：" + ex.Message);
            str.Append("\r\n\t錯誤源：" + ex.Source);
            str.Append("\r\n\t異常方法：" + ex.TargetSite);
            str.Append("\r\n\t堆棧信息：" + ex.StackTrace);
            str.Append("\r\n--------------------------------------------------------------------------------------------------");
            string path = Server.MapPath(WebConfigurationManager.AppSettings["errorLogPath"]);
            AppData.WriteLog(path,str.ToString());
            Response.Redirect("~/Login/Error");
        }
    }
}