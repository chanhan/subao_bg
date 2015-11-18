using IServices;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Linq.Expressions;
using Common;
namespace Services
{
    public class LoginService : RepositoryBase<Employee>, ILoginService
    {
        public LoginService(IDatabaseFactory databaseFactory, IUser user)
            : base(databaseFactory, user)
        {
        }

        public Employee Login(Employee employee)
        {
            string[] ranks = AppData.LoginRanetks.Split(',');
            string password = MD5Password.GetMD5Password(employee.Password);
            Employee user = base.QueryByCondition(e => e.EmployeeName == employee.EmployeeName && e.Password == password && e.Active == "Y" && ranks.Contains(e.Rank)).FirstOrDefault();
            return user;
        }


        public bool IpAccess_Check(Employee employee)
        {
            if (employee.Rank != "1" && employee.Rank != "4")
            {
                //只验证 操盘手 和建资料 帐号
                return true;
            }
            //完整IP
            string Ip = GetClientIp();
            // ip 解析失敗
            System.Net.IPAddress ipAddress;
            if (!System.Net.IPAddress.TryParse(Ip, out ipAddress)) { return false; }

            // Loopback (localhost) 直接過驗證
            if (System.Net.IPAddress.IsLoopback(ipAddress)) { return true; }

            //127.0.0.1  ==>127.0.0
            string temp = Ip.Substring(0, Ip.LastIndexOf("."));

            if (db.IPAccess.Where(p => p.IP.Contains("*") ? p.IP.Contains(temp) : p.IP == Ip).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //完整Ip
        private static string GetClientIp()
        {
            string ip = null;
            System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
            string forward = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(forward))
            {
                ip = request.ServerVariables["REMOTE_ADDR"];
            }
            else if (forward.IndexOf(",") > 0)
            {
                ip = forward.Substring(1, forward.IndexOf(",") - 1);
            }
            else if (forward.IndexOf(";") > 0)
            {
                ip = forward.Substring(1, forward.IndexOf(";") - 1);
            }
            else
            {
                ip = forward;
            }

            return ip.Replace(" ", String.Empty);
        }
    }
}
