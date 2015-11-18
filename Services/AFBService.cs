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
    public class AFBService : RepositoryBase<AFBSchedules>, IAFBService
    {
        private readonly IScoreModifyRecordService scoreModifyRecord;
        private readonly IModifyRecordService modifyRecord;
        public AFBService(IDatabaseFactory databaseFactory, IUser IUserService, IScoreModifyRecordService IScoreModifyRecord, IModifyRecordService IModifyRecordService)
            : base(databaseFactory, IUserService)
        {
            scoreModifyRecord = IScoreModifyRecord;
            modifyRecord = IModifyRecordService;
        }
        public List<AFB> GetAFBSchedules(string gameType, int? gid, DateTime date)
        {
            var linq = from afb in db.AFBSchedules.Where(p => !p.IsDeleted && p.GameType == gameType && p.GameDate.CompareTo(date) == 0 && (gid == null ? true : gid.Value == p.GID))
                       join alliance in db.AFBAlliance.Where(p => !p.IsDeleted && p.GameType == gameType) on afb.AllianceID equals alliance.AllianceID into al
                       from afbAlliance in al.DefaultIfEmpty()
                       join teama in db.AFBTeam.Where(p => !p.IsDeleted && p.GameType == gameType) on afb.TeamAID equals teama.TeamID into ta
                       from afbTeama in ta.DefaultIfEmpty()
                       join teamb in db.AFBTeam.Where(p => !p.IsDeleted && p.GameType == gameType) on afb.TeamBID equals teamb.TeamID into tb
                       from afbTeamb in tb.DefaultIfEmpty()
                       //join source in db.SourceType.Where(p => p.GameType == gameType)
                       //on basketballTeama.SourceID equals source.SourceID into st
                       //from sourceType in st.DefaultIfEmpty(
                       //    //new SourceType {
                       //    //    GameSource = "無"
                       //    //}
                       //)
                       join alliancea in db.AFBAlliance.Where(p => !p.IsDeleted) on afbTeama.AllianceID equals alliancea.AllianceID into taa
                       from taalliance in taa.DefaultIfEmpty()
                       join allianceb in db.AFBAlliance.Where(p => !p.IsDeleted) on afbTeamb.AllianceID equals allianceb.AllianceID into tba
                       from tballiance in tba.DefaultIfEmpty()
                       orderby afb.Number, afb.GameTime
                       select new AFB
                       {
                           AllianceID = afb.AllianceID,
                           AllianceName = afbAlliance.AllianceName,
                           CtrlAdmin = afb.CtrlAdmin,
                           TeamAName = afbTeama.ShowName,
                           TeamBName = afbTeamb.ShowName,
                           CtrlStates = afb.CtrlStates,
                           GameDate = afb.GameDate,
                           GameTime = afb.GameTime,
                           //    GameSource = !string.IsNullOrEmpty(sourceType.GameSource) ? sourceType.GameSource : "無",
                           //  SourceID = sourceType.SourceID,
                           GameStates = afb.GameStates,
                           GameType = afb.GameType,
                           GID = afb.GID,
                           Number = afb.Number,
                           OrderBy = afb.OrderBy,
                           ShowJS = afb.ShowJS,
                           IsDeleted = afb.IsDeleted,
                           TeamAAlliance = !string.IsNullOrEmpty(taalliance.AllianceName) ? taalliance.AllianceName : string.Empty,
                           TeamBAlliance = !string.IsNullOrEmpty(tballiance.AllianceName) ? tballiance.AllianceName : string.Empty,
                           TeamAID = afbTeama.TeamID,
                           TeamBID = afbTeamb.TeamID,
                           WebID = afb.WebID,
                           TeamAAllianceID = afbTeama.AllianceID,
                           TeamBAllianceID = afbTeamb.AllianceID,
                           TrackerText = afb.TrackerText
                       };
            return linq.ToList();
        }
        public int SetShowJs(IList<AFBSchedules> shedules)
        {
            string gameType = shedules.First().GameType;
            string Identifier = MD5Password.GenerateId();

            List<AFBSchedules> change = shedules.Where(p => p.ShowJSChanged == 1).ToList();
            List<int> gids = change.Select(p => p.GID).ToList();
            List<AFBSchedules> schedules = this.QueryByCondition(p => gids.Contains(p.GID)).ToList();
            foreach (var item in schedules)
            {
                AFBSchedules schedule = change.SingleOrDefault(p => p.GID == item.GID);
                if (schedule != null)
                {
                    ModifyRecord record = base.SaveModifyRecord(item, schedule, ActionItem.Update, CategoryItem.Schedule, gameType, Identifier);
                    modifyRecord.Add(record);
                    item.ShowJS = schedule.ShowJS;
                }
            }
            base.Update(schedules);
            return base.Commit();
        }
        public int SetShow(IList<AFBSchedules> shedules)
        {
            string gameType = shedules.First().GameType;
            string Identifier = MD5Password.GenerateId();

            List<AFBSchedules> change = shedules.Where(p => p.IsDeletedChanged == 1).ToList();
            List<int> gids = change.Select(p => p.GID).ToList();
            List<AFBSchedules> schedules = this.QueryByCondition(p => gids.Contains(p.GID)).ToList();
            foreach (var item in schedules)
            {
                AFBSchedules schedule = change.SingleOrDefault(p => p.GID == item.GID);
                if (schedule != null)
                {
                    ModifyRecord record = base.SaveModifyRecord(item, schedule, ActionItem.Update, CategoryItem.Schedule, gameType, Identifier);
                    modifyRecord.Add(record);

                    item.IsDeleted = schedule.IsDeleted;
                }
            }
            base.Update(schedules);
            return base.Commit();
        }
        public int EditSchedule(AFBSchedules basketball)
        {
            int n = CheckData(basketball);
            if (n <= 0) return n;

            string gameType = basketball.GameType;
            string Identifier = MD5Password.GenerateId();
            //新增赛事
            if (basketball.GID == 0)
            {
                basketball.CreateTime = DateTime.Now;
                base.Add(basketball);
                base.Commit();
                ModifyRecord record = base.SaveModifyRecord(null, basketball, ActionItem.Add, CategoryItem.Schedule, gameType, Identifier);
                modifyRecord.Add(record);
            }
            //修改赛事
            else
            {
                AFBSchedules schedule = base.QueryById(basketball.GID);

                ModifyRecord record = base.SaveModifyRecord(schedule, basketball, ActionItem.Update, CategoryItem.Schedule, gameType, Identifier);
                modifyRecord.Add(record);

                schedule.AllianceID = basketball.AllianceID;
                schedule.CtrlStates = basketball.CtrlStates;
                schedule.GameDate = basketball.GameDate;
                schedule.GameStates = basketball.GameStates;
                schedule.GameTime = basketball.GameTime;
                schedule.Number = basketball.Number;
                schedule.ShowJS = basketball.ShowJS;
                schedule.TeamAID = basketball.TeamAID;
                schedule.TeamBID = basketball.TeamBID;
                schedule.TrackerText = basketball.TrackerText;
                schedule.WebID = basketball.WebID;
                schedule.ChangeCount++;

                base.Update(schedule);
            }
            return base.Commit();
        }
        private int CheckData(AFBSchedules afb)
        {
            //队伍选取是否重复
            if (afb.TeamAID == afb.TeamBID)
            {
                return -3;
            }
            //大联盟是否存在
            if (!this.db.AFBAlliance.Where(p => p.AllianceID == afb.AllianceID && !p.IsDeleted).Any())
            {
                return -2;
            }
            //先攻队伍是否存在
            if (!this.db.AFBTeam.Where(p => p.TeamID == afb.TeamAID && !p.IsDeleted).Any())
            {
                return -1;
            }
            //后攻队伍是否存在
            if (!this.db.AFBTeam.Where(p => p.TeamID == afb.TeamAID && !p.IsDeleted).Any())
            {
                return 0;
            }
            return 1;
        }

        public AFBScoreModify GetModifySchedules(int gid)
        {
            var linq = (from s in db.AFBSchedules.Where(p => p.GID == gid)
                        join ta in db.AFBTeam on s.TeamAID equals ta.TeamID
                        join tb in db.AFBTeam on s.TeamBID equals tb.TeamID
                        select new AFBScoreModify
                        {
                            Date = s.GameDate,
                            Time = s.GameTime,
                            GameStates = s.GameStates,
                            GID = s.GID,
                            RA = s.RA,
                            RB = s.RB,
                            RunsA = s.RunsA,
                            RunsB = s.RunsB,
                            TeamAName = ta.ShowName,
                            TeamBName = tb.ShowName,
                            CtrlStates = s.CtrlStates,
                            StatusText = s.StatusText,
                        }).SingleOrDefault();
            if (linq != null)
            {   //未开赛比分为null
                linq.ScoresA = linq.RunsA != null ? linq.RunsA.Split(',').ToList() : new List<string>();
                linq.ScoresB = linq.RunsB != null ? linq.RunsB.Split(',').ToList() : new List<string>();
            }
            return linq;
        }
        public int UpdateScore(AFBSchedules afb)
        {
            AFBSchedules schedule = base.QueryById(afb.GID);
            if (schedule.RunsA == afb.RunsA && schedule.RunsB == afb.RunsB && schedule.RA == afb.RA && schedule.RB == afb.RB && schedule.StatusText == afb.StatusText)
            {
                return -10;
            }
            ScoreModifyRecord record = new ScoreModifyRecord
            {
                GameType = schedule.GameType,
                IpAddr = userService.UserIP,
                ScheduleID = afb.GID,
                ModifyUser = userService.UserName,
                ModifyTime = DateTime.Now,
                RANew = afb.RA,
                RBNew = afb.RB,
                RAOld = schedule.RA,
                RBOld = schedule.RB,
                RunsANew = afb.RunsA,
                RunsBNew = afb.RunsB,
                RunsAOld = schedule.RunsA,
                RunsBOld = schedule.RunsB,
                StatusTextNew = afb.StatusText,
                StatusTextOld = schedule.StatusText
            };  
            scoreModifyRecord.Add(record);
            base.Commit();
            schedule.RA = afb.RA;
            schedule.RB = afb.RB;
            schedule.RunsA = afb.RunsA;
            schedule.RunsB = afb.RunsB;
            schedule.StatusText = afb.StatusText;
            schedule.ChangeCount++;
            if (schedule.CtrlStates == 1)
            {
                schedule.CtrlAdmin = userService.UserName;
            }
            base.Update(schedule);
            return base.Commit();
        }
    }
}
