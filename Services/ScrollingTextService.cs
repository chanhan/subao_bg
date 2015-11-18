using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ScrollingTextService : RepositoryBase<ScrollingText>, IScrollingTextService
    {
        public ScrollingTextService(IDatabaseFactory databaseFactory, IUser userService)
            : base(databaseFactory, userService)
        {
        }
        public int EditScrollingText(ScrollingText scrolling)
        {
            if (scrolling.IsAdd)
            {
                scrolling.Creator = userService.UserName;
                scrolling.CreateTime = DateTime.Now;
                scrolling.Liveticker = 0;
                this.Add(scrolling);
            }
            else
            {
                ScrollingText oldScrollingText = this.QueryById(scrolling.ScrollingTextID);
                oldScrollingText.SettingType = scrolling.SettingType;
                oldScrollingText.LanguageCode = scrolling.LanguageCode;
                oldScrollingText.HyperLink = scrolling.HyperLink;
                oldScrollingText.StartTime = scrolling.StartTime;
                oldScrollingText.EndTime = scrolling.EndTime;
                oldScrollingText.Text = scrolling.Text;
                oldScrollingText.OrderBy = scrolling.OrderBy;
                oldScrollingText.Visible = scrolling.Visible;
                oldScrollingText.Modifier = userService.UserName;
                oldScrollingText.ModifyTime = DateTime.Now;
                this.Update(oldScrollingText);
            }
           return this.Commit();
        }
    }
}
