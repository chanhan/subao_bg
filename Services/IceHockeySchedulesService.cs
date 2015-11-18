using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.ViewModel;
using Models;
using IServices;
using Services.Infrastructure;
using Common;

namespace Services
{
    public class IceHockeySchedulesService : RepositoryBase<IceHockeySchedules>, IIceHockeySchedulesService
    {
        /// <summary>
        /// 赛程修改记录
        /// </summary>
        private readonly IModifyRecordService _mrs;
        /// <summary>
        /// 分数修改记录
        /// </summary>
        private readonly IScoreModifyRecordService _smrs;
        public IceHockeySchedulesService(IDatabaseFactory DatabaseFactory, IUser user, IModifyRecordService mrs, IScoreModifyRecordService smrs)
            : base(DatabaseFactory, user)
        {
            _mrs = mrs;
            _smrs = smrs;
        }


        public List<IceHockey> GetSchedules(DateTime date, string gameType)
        {
            var linq = (from b in db.IceHockeySchedules.Where(p => p.GameDate.CompareTo(date) == 0 && p.GameType == gameType && (gameType.ToUpper() != "IHBF" ? p.Display : true))
                        join a in db.IceHockeyAlliance on b.AllianceID equals a.AllianceID
                        join tA in db.IceHockeyTeam on b.TeamAID equals tA.TeamID
                        join tB in db.IceHockeyTeam on b.TeamBID equals tB.TeamID
                        orderby b.Number, b.GameTime
                        select new IceHockey
                        {
                            GID = b.GID,
                            GameType = b.GameType,
                            GameDate = b.GameDate,
                            Number = b.Number,
                            Alliance = a.ShowName,
                            ShowJS = b.ShowJS,
                            GameTime = b.GameTime,
                            TeamA = tA.ShowName,
                            TeamB = tB.ShowName,
                            GameStates = b.GameStates,
                            WebID = b.WebID,
                            CtrlStates = b.CtrlStates,
                            CtrlAdmin = b.CtrlAdmin,
                            Display = b.Display,
                            IsScoreModifyRecord = (b.CtrlAdmin != null),//表示有人手动修改过比分
                            //IsScoreModifyRecord = (db.ScoreModifyRecord.Count(p => p.ScheduleID == b.GID) > 0),
                            TrackerText = b.TrackerText,
                            AllianceDisplay = a.Display
                        }
                          );

            return linq.ToList();
        }

        private int CheckData(IceHockeySchedules ih)
        {
            //队伍选取是否重复
            if (ih.TeamAID == ih.TeamBID)
            {
                return -3;
            }
            //大联盟是否存在
            if (!this.db.IceHockeyAlliance.Where(p => p.AllianceID == ih.AllianceID && p.Display).Any())
            {
                return -2;
            }
            //先攻队伍是否存在
            if (!this.db.IceHockeyTeam.Where(p => p.TeamID == ih.TeamAID && p.Display).Any())
            {
                return -1;
            }
            //后攻队伍是否存在
            if (!this.db.IceHockeyTeam.Where(p => p.TeamID == ih.TeamAID && p.Display).Any())
            {
                return 0;
            }
            return 1;
        }

        public IceHockey GetSchedulesByID(int GID)
        {
            var linq = (from b in db.IceHockeySchedules.Where(p => p.GID == GID)
                        join a in db.IceHockeyAlliance on b.AllianceID equals a.AllianceID
                        join tA in db.IceHockeyTeam on b.TeamAID equals tA.TeamID
                        join tB in db.IceHockeyTeam on b.TeamBID equals tB.TeamID
                        orderby b.GameTime
                        select new IceHockey
                        {
                            GID = b.GID,
                            GameType = b.GameType,
                            Number = b.Number,
                            Alliance = a.AllianceName,
                            ShowJS = b.ShowJS,
                            GameTime = b.GameTime,
                            GameDate = b.GameDate,
                            TeamA = tA.ShowName,
                            TeamB = tB.ShowName,
                            GameStates = b.GameStates,
                            WebID = b.WebID,
                            CtrlStates = b.CtrlStates,
                            Display = b.Display,
                            IsScoreModifyRecord = (db.ScoreModifyRecord.Count(p => p.ScheduleID == b.GID) > 0)
                        }
                         ).ToList();

            if (linq.Count > 0)
            {
                return linq[0];
            }
            else
            {
                return null;
            }
        }

        public int SaveSetting(IEnumerable<IceHockey> models, Common.SetItem si)
        {
            IEnumerable<int> gids = models.Select(p => p.GID);
            List<IceHockeySchedules> bs = QueryByCondition(p => gids.Contains(p.GID)).ToList();
            IceHockey schedule;
            IceHockeySchedules temp;
            string MD5ID = Common.MD5Password.GenerateId();
            switch (si)
            {
                case Common.SetItem.DeleteSelectionProject:
                    ModifyRecord record;
                    foreach (var item in bs)
                    {
                        schedule = null;
                        schedule = models.SingleOrDefault(p => p.GID == item.GID);
                        if (schedule != null && item.Display == schedule.Display)
                        {
                            record = null;
                            record = base.SaveModifyRecord(item, null, ActionItem.Delete, CategoryItem.Schedule, item.GameType, MD5ID);
                            _mrs.Add(record);
                            item.Display = !schedule.Display;
                        }
                    }
                    break;
                case Common.SetItem.StorageReschedulingSetting:
                    //冰球没有补赛
                    break;
                case Common.SetItem.ChartStorageSet:
                    foreach (var item in bs)
                    {
                        schedule = null;
                        schedule = models.SingleOrDefault(p => p.GID == item.GID);
                        if (schedule != null && item.ShowJS != schedule.ShowJS)
                        {
                            record = null;
                            temp = copy(item);
                            temp.ShowJS = schedule.ShowJS;
                            record = base.SaveModifyRecord(item, temp, ActionItem.Update, CategoryItem.Schedule, item.GameType, MD5ID);
                            _mrs.Add(record);
                            item.ShowJS = schedule.ShowJS;
                        }
                    }
                    break;
                default:
                    break;
            }
            Update(bs);


            return this.Commit();
        }

        public void ScoreModifyRecords(IceHockeySchedules oldModel, IceHockeySchedules newModel)
        {
            ScoreModifyRecord modelSmr = new ScoreModifyRecord()
            {
                ScheduleID = oldModel.GID,
                GameType = oldModel.GameType,
                RunsAOld = oldModel.RunsA,
                RunsBOld = oldModel.RunsB,
                RAOld = oldModel.RA,
                RBOld = oldModel.RB,
                StatusTextOld = Common.AppData.GetStatesText(oldModel.GameStates),
                RunsANew = newModel.RunsA,
                RunsBNew = newModel.RunsB,
                RANew = newModel.RA,
                RBNew = newModel.RB,
                StatusTextNew = Common.AppData.GetStatesText(newModel.GameStates),
                ModifyTime = DateTime.Now,
                ModifyUser = userService.UserName,
                IpAddr = userService.UserIP,
                ModifyItem = newModel.CtrlStates,
                GameDateOld = oldModel.GameDate.ToString("yyyy-MM-dd"),
                GameTimeOld = oldModel.GameTime.ToString(@"hh\:mm"),
                GameDateNew = newModel.GameDate.ToString("yyyy-MM-dd"),
                GameTimeNew = newModel.GameTime.ToString(@"hh\:mm"),
            };
            _smrs.Add(modelSmr);
        }

        public int CreateSchedules(IceHockeySchedules ih, string gameType)
        {
            int x = CheckData(ih);
            if (x < 1)
            {
                return x;
            }

            ih.CreateTime = DateTime.Now;

            ModifyRecord record = base.SaveModifyRecord(ih, ih, ActionItem.Add, CategoryItem.Schedule, gameType, MD5Password.GenerateId());
            _mrs.Add(record);

            base.Add(ih);

            return Commit();
        }

        public int EditSchedules(IceHockeySchedules ih)
        {
            int x = CheckData(ih);
            if (x < 1)
            {
                return x;
            }
            IceHockeySchedules old = QueryById(ih.GID);
            ih.Display = old.Display;
            ModifyRecord record = base.SaveModifyRecord(old, ih, ActionItem.Update, CategoryItem.Schedule, ih.GameType, MD5Password.GenerateId());
            _mrs.Add(record);

            old.Number = ih.Number;
            old.AllianceID = ih.AllianceID;
            old.GameDate = ih.GameDate;
            old.GameTime = ih.GameTime;
            old.TeamAID = ih.TeamAID;
            old.TeamBID = ih.TeamBID;
            old.GameStates = ih.GameStates;
            old.ShowJS = ih.ShowJS;
            old.CtrlStates = ih.CtrlStates;
            old.WebID = ih.WebID;
            old.TrackerText = ih.TrackerText;
            old.ChangeCount++;

            Update(old);
            return Commit();
        }

        public int EditScore(IceHockeySchedules ih)
        {
            IceHockeySchedules old = QueryById(ih.GID);
            ScoreModifyRecords(old, ih);

            if (old.RunsA == ih.RunsA &&
                old.RunsB == ih.RunsB &&
                old.RA == ih.RA &&
                old.RB == ih.RB &&
                old.StatusText == ih.StatusText)
            {
                return -10;
            }
            old.RunsA = ih.RunsA;
            old.RunsB = ih.RunsB;
            old.RA = ih.RA;
            old.RB = ih.RB;
            old.StatusText = ih.StatusText;
            old.ChangeCount++;
            old.CtrlAdmin = userService.UserName;

            Update(old);
            return Commit();
        }

        public int SaveSetting(IEnumerable<IceHockey> models)
        {
            IEnumerable<int> gids = models.Select(p => p.GID);
            List<IceHockeySchedules> IHBFs = QueryByCondition(p => gids.Contains(p.GID)).ToList();
            IceHockey schedule;
            ModifyRecord record;
            IceHockeySchedules temp;
            string MD5ID = Common.MD5Password.GenerateId();
            foreach (var item in IHBFs)
            {
                schedule = null;
                schedule = models.SingleOrDefault(p => p.GID == item.GID);
                //只有当显示设定、走势设定、状态文字有更改至少一项时才会修改
                if (schedule != null && (item.Display != schedule.Display || item.ShowJS != schedule.ShowJS || item.TrackerText != schedule.TrackerText))
                {
                    record = null;
                    temp = copy(item);
                    temp.ShowJS = schedule.ShowJS;
                    temp.Display = schedule.Display;
                    temp.TrackerText = schedule.TrackerText;
                    record = base.SaveModifyRecord(item, temp, ActionItem.Update, CategoryItem.Schedule, "IHBF", MD5ID);
                    _mrs.Add(record);
                    item.Display = schedule.Display;
                    item.ShowJS = schedule.ShowJS;
                    item.TrackerText = schedule.TrackerText;
                    item.ChangeCount++;
                }
            }
            Update(IHBFs);

            return this.Commit();
        }


        public int EditScoreByIHBF(IceHockeySchedules ihbf)
        {
            IceHockeySchedules old = QueryById(ihbf.GID);
            ScoreModifyRecords(old, ihbf);

            //CtrlStates 修改全部：4 修改狀態：3 修改比分：1
            old.CtrlStates = ihbf.CtrlStates;
            switch (ihbf.CtrlStates)
            {
                case 4:
                    old.RunsA = ihbf.RunsA;
                    old.RunsB = ihbf.RunsB;
                    old.RA = ihbf.RA;
                    old.RB = ihbf.RB;
                    old.GameDate = ihbf.GameDate;
                    old.GameTime = ihbf.GameTime;
                    old.GameStates = ihbf.GameStates;
                    old.CtrlStates = ihbf.CtrlStates;
                    break;
                case 3:
                    old.GameDate = ihbf.GameDate;
                    old.GameTime = ihbf.GameTime;
                    old.GameStates = ihbf.GameStates;
                    old.CtrlStates = ihbf.CtrlStates;

                    break;
                case 1:
                    old.RunsA = ihbf.RunsA;
                    old.RunsB = ihbf.RunsB;
                    old.RA = ihbf.RA;
                    old.RB = ihbf.RB;
                    break;
                default:
                    break;
            }
            if (ihbf.GameStates.ToUpper() == "X")
            {
                old.RunsA = null;
                old.RunsB = null;
                old.RA = 0;
                old.RB = 0;
            }
            old.ChangeCount++;
            old.CtrlAdmin = userService.UserName;

            Update(old);
            return Commit();
        }


        public int DeleteScore(IceHockeySchedules ihbf)
        {
            IceHockeySchedules old = QueryById(ihbf.GID);
            ScoreModifyRecord modelSmr = new ScoreModifyRecord()
            {
                ScheduleID = ihbf.GID,
                GameType = ihbf.GameType,
                ModifyTime = DateTime.Now,
                ModifyUser = userService.UserName,
                IpAddr = userService.UserIP,
                ModifyItem = 5,
            };
            _smrs.Add(modelSmr);

            old.ChangeCount++;
            old.CtrlStates = 2;
            old.CtrlAdmin = userService.UserName;

            Update(old);
            return this.Commit();
        }

        private IceHockeySchedules copy(IceHockeySchedules ih)
        {
            IceHockeySchedules newIh = new IceHockeySchedules()
            {
                GID = ih.GID,
                OrderBy = ih.OrderBy,
                AllianceID = ih.AllianceID,
                GameDate = ih.GameDate,
                GameStates = ih.GameStates,
                GameTime = ih.GameTime,
                GameType = ih.GameType,
                Number = ih.Number,
                Display = ih.Display,
                ShowJS = ih.ShowJS,
                TrackerText = ih.TrackerText,
                TeamAID = ih.TeamAID,
                TeamBID = ih.TeamBID,
                CtrlStates = ih.CtrlStates,
                WebID = ih.WebID
            };
            return newIh;
        }
    }
}
