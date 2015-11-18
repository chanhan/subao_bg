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
    public class BKOSService : RepositoryBase<BasketballSchedules>, IBKOSService
    {

        private readonly IModifyRecordService modifyRecord;
        private readonly IScoreModifyRecordService scoreModifyRecord;
        public BKOSService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser IUserService, IScoreModifyRecordService IScoreModifyRecord)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = modifyRecordService;
            scoreModifyRecord = IScoreModifyRecord;
        }

        /// <summary>
        /// 按时间查找奥讯赛事
        /// </summary>
        /// <param name="schedulesDate"></param>
        /// <returns></returns>
        public List<BKOSSchedules> GetBKOSSchedules(DateTime schedulesDate)
        {
            var linq = from s in db.BasketballSchedules.Where(p => p.GameType == "BKOS" && p.GameDate.CompareTo(schedulesDate) == 0)
                       join a in db.OSAlliance on s.AllianceID equals a.AllianceID
                       join ta in db.OSTeam on s.TeamAID equals ta.TeamID
                       join tb in db.OSTeam on s.TeamBID equals tb.TeamID
                       orderby a.ShowName, s.GameTime
                       select new BKOSSchedules
                       {
                           GID = s.GID,
                           AllianceID = s.AllianceID,
                           AllianceName = a.ShowName,
                           AllianceDisPlay = a.Display,
                           IsDeleted = s.IsDeleted,
                           TeamAName = ta.ShowName,
                           TeamBName = tb.ShowName,
                           Date = s.GameDate,
                           Time = s.GameTime,
                           GameStates = s.GameStates,
                           ShowJS = s.ShowJS,
                           TrackerText = s.TrackerText,
                           CtrlStates = s.CtrlStates,
                           TeamAID = s.TeamAID,
                           TeamBID = s.TeamBID,
                           GameType = s.GameType
                       };
            return linq.ToList();
        }
        public BKOSSchedules GetBKOSScheduleByID(int gid)
        {
            var linq = from s in db.BasketballSchedules.Where(p => p.GameType == "BKOS" && p.GID == gid)
                       join a in db.OSAlliance on s.AllianceID equals a.AllianceID
                       join ta in db.OSTeam on s.TeamAID equals ta.TeamID
                       join tb in db.OSTeam on s.TeamBID equals tb.TeamID
                       orderby a.ShowName, s.GameTime
                       select new BKOSSchedules
                       {
                           GID = s.GID,
                           AllianceID = s.AllianceID,
                           AllianceName = a.ShowName,
                           AllianceDisPlay = a.Display,
                           IsDeleted = !s.IsDeleted,
                           TeamAName = ta.ShowName,
                           TeamBName = tb.ShowName,
                           Date = s.GameDate,
                           Time = s.GameTime,
                           GameStates = s.GameStates,
                           ShowJS = s.ShowJS,
                           TrackerText = s.TrackerText,
                           CtrlStates = s.CtrlStates,
                           TeamAID = s.TeamAID,
                           TeamBID = s.TeamBID
                       };
            return linq.SingleOrDefault();
        }

        /// <summary>
        /// 保存奥讯显示设定、走势设定
        /// </summary>
        /// <param name="bkos"></param>
        /// <returns></returns>
        public int SaveSchedules(IList<BasketballSchedules> bkos)
        {
            List<BasketballSchedules> change = bkos.Where(p => (p.IsDeletedChanged + p.ShowJSChanged + p.TrackerTextChanged) > 0).ToList();
            List<int> gids = change.Select(p => p.GID).ToList();
            List<BasketballSchedules> schedules = this.QueryByCondition(p => gids.Contains(p.GID)).ToList();
            string gameType = "BKOS";
            string Identifier = MD5Password.GenerateId();
            foreach (var item in schedules)
            {
                BasketballSchedules schedule = change.SingleOrDefault(p => p.GID == item.GID);
                schedule.IsDeleted = !schedule.IsDeleted;
                ModifyRecord record = base.SaveModifyRecord(item, schedule, ActionItem.Update, CategoryItem.Schedule, gameType, Identifier);
                modifyRecord.Add(record);
                if (schedule != null)
                {
                    item.IsDeleted = schedule.IsDeleted;
                    item.ShowJS = schedule.ShowJS;
                    if (item.TrackerText != schedule.TrackerText)
                    {
                        item.TrackerText = schedule.TrackerText;
                        item.IsTracker = true;
                    }
                }
            }
            this.Update(schedules);
            return base.Commit();
        }
        /// <summary>
        /// 按id查找奥讯赛事
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public BKOSScoreModify GetModifySchedules(int gid)
        {
            var linq = (from s in db.BasketballSchedules.Where(p => p.GID == gid)
                        join ta in db.OSTeam on s.TeamAID equals ta.TeamID
                        join tb in db.OSTeam on s.TeamBID equals tb.TeamID
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
                            StatusText = s.StatusText
                        }).SingleOrDefault();
            if (linq != null)
            {   //未开赛比分为null
                linq.ScoresA = linq.RunsA != null ? linq.RunsA.Split(',').ToList() : new List<string>();
                linq.ScoresB = linq.RunsB != null ? linq.RunsB.Split(',').ToList() : new List<string>();
            }
            return linq;
        }

        public int UpdateScore(BasketballSchedules basketball, int modifyItem)
        {
            BasketballSchedules bkos = base.QueryById(basketball.GID);
            ScoreModifyRecord record = new ScoreModifyRecord
            {
                GameType = "BKOS",
                IpAddr = userService.UserIP,
                ScheduleID = basketball.GID,
                ModifyUser = userService.UserName,
                ModifyTime = DateTime.Now,
                ModifyItem = modifyItem
            };
            //比赛状态
            if (modifyItem == 1)
            {
                record.StatusTextOld = Common.AppData.GetStatesText(bkos.GameStates);
                record.StatusTextNew = Common.AppData.GetStatesText(basketball.GameStates);

                bkos.GameStates = basketball.GameStates;
                if (basketball.GameStates == "X")
                {
                    record.RAOld = bkos.RA;
                    record.RANew = 0;
                    record.RBOld = bkos.RB;
                    record.RBNew = 0;
                    record.RunsAOld = bkos.RunsA;
                    record.RunsANew = null;
                    record.RunsBOld = bkos.RunsB;
                    record.RunsBNew = null;
                    
                    bkos.RunsA = null;
                    bkos.RunsB = null;
                    bkos.RA = 0;
                    bkos.RB = 0;
                }
            }
            //时间
            else if (modifyItem == 2)
            {
                record.GameDateOld = bkos.GameDate.ToString("yyyy-MM-dd");
                record.GameDateNew = basketball.GameDate.ToString("yyyy-MM-dd");
                record.GameTimeOld = bkos.GameTime.ToString(@"hh\:mm");
                record.GameTimeNew = basketball.GameTime.ToString(@"hh\:mm");

                bkos.GameDate = basketball.GameDate;
                bkos.GameTime = basketball.GameTime;
            }
            //分数
            else if (modifyItem == 3)
            {
                record.RAOld = bkos.RA;
                record.RANew = basketball.RA;
                record.RBOld = bkos.RB;
                record.RBNew = basketball.RB;
                record.RunsAOld = bkos.RunsA;
                record.RunsANew = basketball.RunsA;
                record.RunsBOld = bkos.RunsB;
                record.RunsBNew = basketball.RunsB;

                bkos.RunsA = basketball.RunsA;
                bkos.RunsB = basketball.RunsB;
                bkos.RA = basketball.RA;
                bkos.RB = basketball.RB;
            }
            //全部
            else if (modifyItem == 4)
            {
                record.StatusTextOld = Common.AppData.GetStatesText(bkos.GameStates);
                record.StatusTextNew = Common.AppData.GetStatesText(basketball.GameStates);

                record.GameDateOld = bkos.GameDate.ToString("yyyy-MM-dd");
                record.GameDateNew = basketball.GameDate.ToString("yyyy-MM-dd");
                record.GameTimeOld = bkos.GameTime.ToString(@"hh\:mm");
                record.GameTimeNew = basketball.GameTime.ToString(@"hh\:mm");

                bkos.GameStates = basketball.GameStates;
                bkos.GameDate = basketball.GameDate;
                bkos.GameTime = basketball.GameTime;

                if (basketball.GameStates == "X")
                {
                    record.RAOld = bkos.RA;
                    record.RANew = 0;
                    record.RBOld = bkos.RB;
                    record.RBNew = 0;
                    record.RunsAOld = bkos.RunsA;
                    record.RunsANew = null;
                    record.RunsBOld = bkos.RunsB;
                    record.RunsBNew = null;

                    bkos.RunsA = null;
                    bkos.RunsB = null;
                    bkos.RA = 0;
                    bkos.RB = 0;
                }
                else
                {
                    record.RAOld = bkos.RA;
                    record.RANew = basketball.RA;
                    record.RBOld = bkos.RB;
                    record.RBNew = basketball.RB;
                    record.RunsAOld = bkos.RunsA;
                    record.RunsANew = basketball.RunsA;
                    record.RunsBOld = bkos.RunsB;
                    record.RunsBNew = basketball.RunsB;

                    bkos.RunsA = basketball.RunsA;
                    bkos.RunsB = basketball.RunsB;
                    bkos.RA = basketball.RA;
                    bkos.RB = basketball.RB;
                }
            }
            bkos.CtrlStates = modifyItem;
            bkos.ChangeCount++;
            base.Update(bkos);
            scoreModifyRecord.Add(record);
            return base.Commit();
        }

        public int DeleteScore(int gid)
        {
            BasketballSchedules bkos = base.QueryById(gid);
            ScoreModifyRecord record = new ScoreModifyRecord
            {
                GameType = "BKOS",
                IpAddr = userService.UserIP,
                ScheduleID = bkos.GID,
                ModifyUser = userService.UserName,
                ModifyTime = DateTime.Now,
                ModifyItem = 5
            };
            bkos.CtrlStates = 0;
            base.Update(bkos);
            scoreModifyRecord.Add(record);
            return base.Commit();
        }
    }
}
