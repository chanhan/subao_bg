using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace System.Web.Mvc
{
    public static class PageHelper
    {
        /// <summary>
        /// 单条件分页查询
        /// </summary>
        public static HtmlString PageList(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, string keyWords)
        {
            if (totalCount == 0) return new HtmlString(string.Empty);
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            var parameter = string.Empty;
            if (!string.IsNullOrEmpty(keyWords))
            {
                parameter = string.Format("&{0}={1}", "keyWords", keyWords);
            }
            return new HtmlString(GetPageHtml(redirectTo, currentPage, pageSize, totalCount, parameter));
        }

        /// <summary>
        /// 多条件分页查询
        /// </summary>
        public static HtmlString PageListFor<T>(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, T t)
        {
            if (totalCount == 0) return new HtmlString(string.Empty);
            Type type = t.GetType();
            string link = string.Empty;
            var parameter = new StringBuilder();
            foreach (System.Reflection.PropertyInfo mi in type.GetProperties())
            {
                object value = mi.GetValue(t, null);
                if (value != null && value.ToString() != "")
                {
                    parameter.AppendFormat("&{0}={1}", mi.Name, value);
                }
            }
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new HtmlString(GetPageHtml(redirectTo, currentPage, pageSize, totalCount, parameter.ToString()));
        }

        /// <summary>
        /// 生成HTML
        /// </summary>
        private static string GetPageHtml(string redirectTo, int currentPage, int pageSize, int totalCount, string keyWords)
        {
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            string link = string.Empty;
            link = "<a href='{0}?pageIndex={1}&pageSize={2}{3}'>{4}</a>";
            if (totalPages > 1)
            {
                output.AppendFormat(link, redirectTo, 1, pageSize, keyWords, "首頁");
                output.Append(" ");
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat(link, redirectTo, currentPage - 1, pageSize, keyWords, "上一頁");
                }
                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理                            
                            output.AppendFormat("{0}", currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat(link, redirectTo, currentPage + i - currint, pageSize, keyWords, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat(link, redirectTo, currentPage + 1, pageSize, keyWords, "下一頁");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat(link, redirectTo, totalPages, pageSize, keyWords, "尾頁");
                }
                output.Append(" ");
            }
            output.AppendFormat("<label>第{0}頁 / 共{1}頁</label>", currentPage, totalPages);
            return output.ToString();
        }
    }
}