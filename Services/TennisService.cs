using Common;
using IServices;
using Models;
using Models.ViewModel;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class TennisService : RepositoryBase<TennisSchedules>, ITennisService
    {
        private readonly IModifyRecordService modifyRecord;
        public TennisService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser user)
            : base(databaseFactory, user)
        {
            modifyRecord = modifyRecordService;
        }
        public List<Tennis> GetTennisSchedules(DateTime date)
        {
            var linq = from s in db.TennisSchedules.Where(p => p.GameDate.CompareTo(date) == 0)
                       join a in db.TNAlliance on s.AllianceID equals a.AllianceID
                       join ta in db.NameControl.Where(p => p.GTLangx == "TN" && p.GameType == "Name")
                       on s.TeamAID equals ta.ID
                       join tb in db.NameControl.Where(p => p.GTLangx == "TN" && p.GameType == "Name")
                       on s.TeamBID equals tb.ID
                       orderby s.OrderBy,s.GameTime
                       select new Tennis
                       {
                           GID = s.GID,
                           AllianceID = s.AllianceID,
                           AllianceName = a.AllianceName,
                           AllianceShowName=a.ShowName,
                           AllianceDisPlay = a.Display,
                           IsDeleted = s.IsDeleted,
                           TeamAID=ta.ID,
                           TeamBID = tb.ID,
                           TeamAName = ta.ChangeText,
                           TeamBName = tb.ChangeText,
                           GameDate = s.GameDate,
                           GameTime = s.GameTime,
                           GameStates = s.GameStates
                       };
            return linq.ToList();
        }
        public int SaveSchedules(IList<TennisSchedules> tennis)
        {
            List<TennisSchedules> change = tennis.Where(p => p.Changed == 1).ToList();
            List<int> gids = change.Select(p => p.GID).ToList();
            List<TennisSchedules> schedules = this.QueryByCondition(p => gids.Contains(p.GID)).ToList();
            string gameType = "TN";
            string Identifier = MD5Password.GenerateId();
            foreach (var item in schedules)
            {
                TennisSchedules schedule = change.SingleOrDefault(p => p.GID == item.GID);
                schedule.IsDeleted = !schedule.IsDeleted;
                ModifyRecord record = base.SaveModifyRecord(item, schedule, ActionItem.Update, CategoryItem.Schedule, gameType, Identifier);
                modifyRecord.Add(record);
                if (schedule != null)
                {
                    item.IsDeleted = schedule.IsDeleted;
                }
            }
            this.Update(schedules);
            return base.Commit();
        }
    }
}
