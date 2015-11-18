using Common;
using IServices;
using Models;
using Models.ViewModel;
using Services.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class BasketballService : RepositoryBase<BasketballSchedules>, IBasketballService
    {
        private readonly IScoreModifyRecordService scoreModifyRecord;
        private readonly IModifyRecordService modifyRecord;
        public BasketballService(IDatabaseFactory databaseFactory, IUser IUserService, IScoreModifyRecordService IScoreModifyRecord, IModifyRecordService IModifyRecordService)
            : base(databaseFactory, IUserService)
        {
            scoreModifyRecord = IScoreModifyRecord;
            modifyRecord = IModifyRecordService;
        }
        public List<Models.ViewModel.BasketBall> GetBasketballSchedules(string gameType, int? gid, DateTime date)
        {
            var linq = from bk in db.BasketballSchedules.Where(p => !p.IsDeleted && p.GameType == gameType && p.GameDate.CompareTo(date) == 0 && (gid == null ? true : gid.Value == p.GID))
                       join alliance in db.BasketballAlliance.Where(p => !p.IsDeleted && p.GameType == gameType) on bk.AllianceID equals alliance.AllianceID into al
                       from basketballAlliance in al.DefaultIfEmpty()
                       join teama in db.BasketballTeam.Where(p => !p.IsDeleted && p.GameType == gameType) on bk.TeamAID equals teama.TeamID into ta
                       from basketballTeama in ta.DefaultIfEmpty()
                       join teamb in db.BasketballTeam.Where(p => !p.IsDeleted && p.GameType == gameType) on bk.TeamBID equals teamb.TeamID into tb
                       from basketballTeamb in tb.DefaultIfEmpty()
                       join source in db.SourceType.Where(p => p.GameType == gameType)
                       on basketballTeama.SourceID equals source.SourceID into st
                       from sourceType in st.DefaultIfEmpty(
                           //new SourceType {
                           //    GameSource = "無"
                           //}
                       )
                       join alliancea in db.BasketballAlliance.Where(p => !p.IsDeleted) on basketballTeama.AllianceID equals alliancea.AllianceID into taa
                       from taalliance in taa.DefaultIfEmpty()
                       join allianceb in db.BasketballAlliance.Where(p => !p.IsDeleted) on basketballTeamb.AllianceID equals allianceb.AllianceID into tba
                       from tballiance in tba.DefaultIfEmpty()
                       orderby bk.Number, bk.GameTime
                       select new BasketBall
                       {
                           AllianceID = bk.AllianceID,
                           AllianceName = basketballAlliance.AllianceName,
                           CtrlAdmin = bk.CtrlAdmin,
                           TeamAName = basketballTeama.ShowName,
                           TeamBName = basketballTeamb.ShowName,
                           CtrlStates = bk.CtrlStates,
                           GameDate = bk.GameDate,
                           GameTime = bk.GameTime,
                           GameSource = !string.IsNullOrEmpty(sourceType.GameSource) ? sourceType.GameSource : "無",
                           SourceID = sourceType.SourceID,
                           GameStates = bk.GameStates,
                           GameType = bk.GameType,
                           GID = bk.GID,
                           Number = bk.Number,
                           OrderBy = bk.OrderBy,
                           ShowJS = bk.ShowJS,
                           IsDeleted = bk.IsDeleted,
                           TeamAAlliance = !string.IsNullOrEmpty(taalliance.AllianceName) ? taalliance.AllianceName : string.Empty,
                           TeamBAlliance = !string.IsNullOrEmpty(tballiance.AllianceName) ? tballiance.AllianceName : string.Empty,
                           TeamAID = basketballTeama.TeamID,
                           TeamBID = basketballTeamb.TeamID,
                           WebID = bk.WebID,
                           TeamAAllianceID = basketballTeama.AllianceID,
                           TeamBAllianceID = basketballTeamb.AllianceID,
                           TrackerText = bk.TrackerText
                       };
            return linq.ToList();
        }
        public int SetShowJs(IList<BasketballSchedules> basketball)
        {
            string gameType = basketball.First().GameType;
            string Identifier = MD5Password.GenerateId();

            List<BasketballSchedules> change = basketball.Where(p => p.ShowJSChanged == 1).ToList();
            List<int> gids = change.Select(p => p.GID).ToList();
            List<BasketballSchedules> schedules = this.QueryByCondition(p => gids.Contains(p.GID)).ToList();
            foreach (var item in schedules)
            {
                BasketballSchedules schedule = change.SingleOrDefault(p => p.GID == item.GID);
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
        public int SetShow(IList<BasketballSchedules> basketball)
        {
            string gameType = basketball.First().GameType;
            string Identifier = MD5Password.GenerateId();

            List<BasketballSchedules> change = basketball.Where(p => p.IsDeletedChanged == 1).ToList();
            List<int> gids = change.Select(p => p.GID).ToList();
            List<BasketballSchedules> schedules = this.QueryByCondition(p => gids.Contains(p.GID)).ToList();
            foreach (var item in schedules)
            {
                BasketballSchedules schedule = change.SingleOrDefault(p => p.GID == item.GID);
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
        public int EditSchedule(BasketballSchedules basketball)
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
                BasketballSchedules schedule = base.QueryById(basketball.GID);

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
        private int CheckData(BasketballSchedules basketball)
        {
            //队伍选取是否重复
            if (basketball.TeamAID == basketball.TeamBID)
            {
                return -3;
            }
            //大联盟是否存在
            if (!this.db.BasketballAlliance.Where(p => p.AllianceID == basketball.AllianceID && !p.IsDeleted).Any())
            {
                return -2;
            }
            //先攻队伍是否存在
            if (!this.db.BasketballTeam.Where(p => p.TeamID == basketball.TeamAID && !p.IsDeleted).Any())
            {
                return -1;
            }
            //后攻队伍是否存在
            if (!this.db.BasketballTeam.Where(p => p.TeamID == basketball.TeamAID && !p.IsDeleted).Any())
            {
                return 0;
            }
            return 1;
        }

        public BKOSScoreModify GetModifySchedules(int gid)
        {
            var linq = (from s in db.BasketballSchedules.Where(p => p.GID == gid)
                        join ta in db.BasketballTeam on s.TeamAID equals ta.TeamID
                        join tb in db.BasketballTeam on s.TeamBID equals tb.TeamID
                        select new BKOSScoreModify
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
        public int UpdateScore(BasketballSchedules basketball)
        {
            BasketballSchedules schedule = base.QueryById(basketball.GID);
            if (schedule.RunsA == basketball.RunsA && schedule.RunsB == basketball.RunsB && schedule.RA == basketball.RA && schedule.RB == basketball.RB && schedule.StatusText == basketball.StatusText)
            {
                return -10;
            }
            ScoreModifyRecord record = new ScoreModifyRecord
            {
                GameType = schedule.GameType,
                IpAddr = userService.UserIP,
                ScheduleID = basketball.GID,
                ModifyUser = userService.UserName,
                ModifyTime = DateTime.Now,
                RANew = basketball.RA,
                RBNew = basketball.RB,
                RAOld = schedule.RA,
                RBOld = schedule.RB,
                RunsANew = basketball.RunsA,
                RunsBNew = basketball.RunsB,
                RunsAOld = schedule.RunsA,
                RunsBOld = schedule.RunsB,
                StatusTextNew = basketball.StatusText,
                StatusTextOld = schedule.StatusText
            }; 
            scoreModifyRecord.Add(record);
            base.Commit();
            schedule.RA = basketball.RA;
            schedule.RB = basketball.RB;
            schedule.RunsA = basketball.RunsA;
            schedule.RunsB = basketball.RunsB;
            schedule.StatusText = basketball.StatusText;
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
