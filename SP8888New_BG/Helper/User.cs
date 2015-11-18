using IServices;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Helper
{
    public class User : IUser
    {
        public string UserName
        {
            get
            {
                string[] user = GetUser();
                if (user.Length == 3)
                {
                    return user[1];
                }
                return string.Empty;
            }
            //set
            //{
            //    HttpContext.Current.Session["userAccount"] = value;
            //}
        }
         public string Sid
        {
            get
            {
                string[] user = GetUser();
                if (user.Length == 3)
                {
                    return user[2];
                }
                return string.Empty;
            }
        }
        public string UserIP
        {
            get
            {
                return GetIpAddress();
            }
        }

        private string[] GetUser()
        {
            return HttpContext.Current.User.Identity.Name.Split(',');
        }
        public string UserRank
        {
            get
            {
                string[] user = GetUser();
                if (user.Length == 3)
                {
                    return user[0];
                }
                return string.Empty;
            }
            //set
            //{
            //    HttpContext.Current.Session["userRank"] = value;
            //}
        }
        private string GetIpAddress()
        {
            string ip = "";
            if (HttpContext.Current.Request.UserHostAddress == "::1")
            {
                ip = "::1";
            }
            else
            {
                ip = HttpContext.Current.Request.UserHostAddress;
                string[] ips = ip.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                ip = String.Format("*.*.{0}.{1}", ips[2], ips[3]);
            }
            return ip;
        }


    }
}