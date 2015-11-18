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
    public class ScoreModifyRecordService : RepositoryBase<ScoreModifyRecord>, IScoreModifyRecordService
    {
        public ScoreModifyRecordService(IDatabaseFactory databaseFactory, IUser IUserService)
            : base(databaseFactory, IUserService)
        {
        }
        public List<BKOSScoreRecord> GetScoreModifyRecord(int gid, string gameType)
        {
            var linq = from s in db.BasketballSchedules.Where(p => p.GameType == gameType && p.GID == gid)
                       join a in db.OSAlliance on s.AllianceID equals a.AllianceID
                       join ta in db.OSTeam on s.TeamAID equals ta.TeamID
                       join tb in db.OSTeam on s.TeamBID equals tb.TeamID
                       join rec in db.ScoreModifyRecord on s.GID equals rec.ScheduleID
                       orderby rec.ModifyTime descending
                       select new BKOSScoreRecord
                       {
                           AllianceName = a.ShowName,
                           TeamAName = ta.ShowName,
                           TeamBName = tb.ShowName,
                           GameDate = s.GameDate,
                           GameDateNew = rec.GameDateNew,
                           GameDateOld = rec.GameDateOld,
                           GameTime = s.GameTime,
                           GameTimeNew = rec.GameTimeNew,
                           GameTimeOld = rec.GameTimeOld,
                           IpAddr = rec.IpAddr,
                           ModifyItem = rec.ModifyItem,
                           ModifyTime = rec.ModifyTime,
                           ModifyUser = rec.ModifyUser,
                           RunsANew = string.IsNullOrEmpty(rec.RunsANew) ? string.Empty : rec.RunsANew,
                           RunsAOld = string.IsNullOrEmpty(rec.RunsAOld) ? string.Empty : rec.RunsAOld,
                           RunsBNew = string.IsNullOrEmpty(rec.RunsBNew) ? string.Empty : rec.RunsBNew,
                           RunsBOld = string.IsNullOrEmpty(rec.RunsBOld) ? string.Empty : rec.RunsBOld,
                           StatusTextNew = rec.StatusTextNew,
                           StatusTextOld = rec.StatusTextOld,
                           RAOld = rec.RAOld,
                           RBOld = rec.RBOld,
                           RANew = rec.RANew,
                           RBNew = rec.RBNew,
                       };
            return linq.ToList();
        }
        public List<BKOSScoreRecord> GetBasketBallScoreModifyRecord(int gid, string gameType)
        {
            var linq = from s in db.BasketballSchedules.Where(p => p.GameType == gameType && p.GID == gid)
                       join a in db.BasketballAlliance on s.AllianceID equals a.AllianceID
                       join ta in db.BasketballTeam on s.TeamAID equals ta.TeamID
                       join tb in db.BasketballTeam on s.TeamBID equals tb.TeamID
                       join rec in db.ScoreModifyRecord on s.GID equals rec.ScheduleID
                       orderby rec.ModifyTime descending
                       select new BKOSScoreRecord
                       {
                           AllianceName = a.AllianceName,
                           TeamAName = ta.ShowName,
                           TeamBName = tb.ShowName,
                           GameDate = s.GameDate,
                           GameDateNew = rec.GameDateNew,
                           GameDateOld = rec.GameDateOld,
                           GameTime = s.GameTime,
                           GameTimeNew = rec.GameTimeNew,
                           GameTimeOld = rec.GameTimeOld,
                           IpAddr = rec.IpAddr,
                           ModifyTime = rec.ModifyTime,
                           ModifyUser = rec.ModifyUser,
                           RunsANew = string.IsNullOrEmpty(rec.RunsANew) ? string.Empty : rec.RunsANew,
                           RunsAOld = string.IsNullOrEmpty(rec.RunsAOld) ? string.Empty : rec.RunsAOld,
                           RunsBNew = string.IsNullOrEmpty(rec.RunsBNew) ? string.Empty : rec.RunsBNew,
                           RunsBOld = string.IsNullOrEmpty(rec.RunsBOld) ? string.Empty : rec.RunsBOld,
                           RAOld = rec.RAOld,
                           RBOld = rec.RBOld,
                           RANew = rec.RANew,
                           RBNew = rec.RBNew,
                       };
            return linq.ToList();
        }
        public List<AFBScoreRecord> GetAFBScoreModifyRecord(int gid, string gameType)
        {
            var linq = from s in db.AFBSchedules.Where(p => p.GameType == gameType && p.GID == gid)
                       join a in db.AFBAlliance on s.AllianceID equals a.AllianceID
                       join ta in db.AFBTeam on s.TeamAID equals ta.TeamID
                       join tb in db.AFBTeam on s.TeamBID equals tb.TeamID
                       join rec in db.ScoreModifyRecord on s.GID equals rec.ScheduleID
                       orderby rec.ModifyTime descending
                       select new AFBScoreRecord
                       {
                           AllianceName = a.AllianceName,
                           TeamAName = ta.ShowName,
                           TeamBName = tb.ShowName,
                           GameDate = s.GameDate,
                           //GameDateNew = rec.GameDateNew,
                           //GameDateOld = rec.GameDateOld,
                           GameTime = s.GameTime,
                           //GameTimeNew = rec.GameTimeNew,
                           //GameTimeOld = rec.GameTimeOld,
                           IpAddr = rec.IpAddr,
                           ModifyTime = rec.ModifyTime,
                           ModifyUser = rec.ModifyUser,
                           RunsANew = string.IsNullOrEmpty(rec.RunsANew) ? string.Empty : rec.RunsANew,
                           RunsAOld = string.IsNullOrEmpty(rec.RunsAOld) ? string.Empty : rec.RunsAOld,
                           RunsBNew = string.IsNullOrEmpty(rec.RunsAOld) ? string.Empty : rec.RunsBNew,
                           RunsBOld = string.IsNullOrEmpty(rec.RunsAOld) ? string.Empty : rec.RunsBOld,
                           RAOld = rec.RAOld,
                           RBOld = rec.RBOld,
                           RANew = rec.RANew,
                           RBNew = rec.RBNew,
                       };
            return linq.ToList();
        }
    }
}
