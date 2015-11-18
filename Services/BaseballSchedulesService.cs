using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Infrastructure;
using Models;
using IServices;
using Models.ViewModel;
using Common;

namespace Services
{
    public class BaseballSchedulesService : RepositoryBase<BaseballSchedules>, IBaseballSchedulesService
    {
        /// <summary>
        /// 赛程修改记录
        /// </summary>
        private readonly IModifyRecordService _mrs;
        /// <summary>
        /// 分数修改记录
        /// </summary>
        private readonly IScoreModifyRecordService _smrs;

        public BaseballSchedulesService(IDatabaseFactory databaseFactory, IUser user, IModifyRecordService mrs, IScoreModifyRecordService smrs)
            : base(databaseFactory, user)
        {
            _mrs = mrs;
            _smrs = smrs;
        }

        public List<Baseball> GetSchedules(DateTime date, string gameType)
        {
            var linq = (from b in db.BaseballSchedules.Where(p => p.GameDate.CompareTo(date) == 0 && p.GameType == gameType && !p.IsDeleted)
                        join a in db.BaseballAlliance on b.AllianceID equals a.AllianceID
                        join tA in db.BaseballTeam on b.TeamAID equals tA.TeamID
                        join tB in db.BaseballTeam on b.TeamBID equals tB.TeamID
                        orderby b.Number, b.GameTime
                        select new Baseball
                        {
                            GID = b.GID,
                            GameType = b.GameType,
                            GameDate = b.GameDate,
                            Number = b.Number,
                            Alliance = a.AllianceName,
                            IsReschedule = b.IsReschedule,
                            ShowJS = b.ShowJS,
                            GameTime = b.GameTime,
                            TeamA = tA.ShowName,
                            TeamB = tB.ShowName,
                            GameStates = b.GameStates,
                            WebID = b.WebID,
                            CtrlStates = b.CtrlStates,
                            CtrlAdmin = b.CtrlAdmin,
                            IsDeleted = b.IsDeleted,
                            IsScoreModifyRecord = (b.CtrlAdmin != null)
                        }
                           );

            return linq.ToList();
        }

        private int CheckData(BaseballSchedules bb)
        {
            //队伍选取是否重复
            if (bb.TeamAID == bb.TeamBID)
            {
                return -3;
            }
            //大联盟是否存在
            if (!this.db.BaseballAlliance.Where(p => p.AllianceID == bb.AllianceID && !p.IsDeleted).Any())
            {
                return -2;
            }
            //先攻队伍是否存在
            if (!this.db.BaseballTeam.Where(p => p.TeamID == bb.TeamAID && !p.IsDeleted).Any())
            {
                return -1;
            }
            //后攻队伍是否存在
            if (!this.db.BaseballTeam.Where(p => p.TeamID == bb.TeamAID && !p.IsDeleted).Any())
            {
                return 0;
            }
            return 1;
        }

        public int SaveSetting(IEnumerable<Models.ViewModel.Baseball> models, Common.SetItem si)
        {
            IEnumerable<int> gids = models.Select(p => p.GID);
            List<BaseballSchedules> bs = QueryByCondition(p => gids.Contains(p.GID)).ToList();
            ModifyRecord record;
            BaseballSchedules temp;
            string MD5ID = Common.MD5Password.GenerateId();
            switch (si)
            {
                case Common.SetItem.DeleteSelectionProject:
                    foreach (var item in bs)
                    {
                        Models.ViewModel.Baseball schedule = models.SingleOrDefault(p => p.GID == item.GID);
                        if (schedule != null && item.IsDeleted != schedule.IsDeleted)
                        {
                            record = base.SaveModifyRecord(item, null, ActionItem.Delete, CategoryItem.Schedule, item.GameType, MD5ID);
                            _mrs.Add(record);
                            item.IsDeleted = schedule.IsDeleted;
                        }
                    }
                    break;
                case Common.SetItem.StorageReschedulingSetting:
                    foreach (var item in bs)
                    {
                        Models.ViewModel.Baseball schedule = models.SingleOrDefault(p => p.GID == item.GID);
                        if (schedule != null && item.IsReschedule != schedule.IsReschedule)
                        {
                            temp = copy(item);
                            temp.IsReschedule = schedule.IsReschedule;
                            record = base.SaveModifyRecord(item, temp, ActionItem.Update, CategoryItem.Schedule, item.GameType, MD5ID);
                            _mrs.Add(record);
                            item.IsReschedule = schedule.IsReschedule;
                        }
                    }
                    break;
                case Common.SetItem.ChartStorageSet:
                    foreach (var item in bs)
                    {
                        Models.ViewModel.Baseball schedule = models.SingleOrDefault(p => p.GID == item.GID);
                        if (schedule != null && item.ShowJS != schedule.ShowJS)
                        {
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


        public int CreateSchedules(BaseballSchedules bb, string gameType)
        {
            bb.OrderBy = 1;
            int x = CheckData(bb);
            if (x < 1)
            {
                return x;
            }

            bb.CreateTime = bb.ChangeTime = DateTime.Now;

            ModifyRecord record = base.SaveModifyRecord(null, bb, ActionItem.Add, CategoryItem.Schedule, gameType, MD5Password.GenerateId());
            _mrs.Add(record);

            base.Add(bb);

            return Commit();
        }

        public int EditSchedules(BaseballSchedules bb)
        {
            int x = CheckData(bb);
            if (x < 1)
            {
                return x;
            }
            BaseballSchedules old = QueryById(bb.GID);
            bb.IsDeleted = old.IsDeleted;
            ModifyRecord record = base.SaveModifyRecord(old, bb, ActionItem.Update, CategoryItem.Schedule, bb.GameType, MD5Password.GenerateId());
            _mrs.Add(record);

            old.Number = bb.Number;
            old.AllianceID = bb.AllianceID;
            old.GameDate = bb.GameDate;
            old.GameTime = bb.GameTime;
            old.TeamAID = bb.TeamAID;
            old.TeamBID = bb.TeamBID;
            old.GameStates = bb.GameStates;
            old.IsReschedule = bb.IsReschedule;
            old.ShowJS = bb.ShowJS;
            old.CtrlStates = bb.CtrlStates;
            old.WebID = bb.WebID;
            old.TrackerText = bb.TrackerText;
            old.ChangeCount++;
            old.ChangeTime = DateTime.Now;
            Update(old);
            return Commit();
        }

        public int EditScore(BaseballSchedules bb)
        {
            BaseballSchedules old = QueryById(bb.GID);
            if (old.RunsA == bb.RunsA &&
               old.RunsB == bb.RunsB &&
               old.RA == bb.RA &&
               old.RB == bb.RB &&
               old.StatusText == bb.StatusText &&
                old.EA == bb.EA &&
                old.HA == bb.HA &&
                old.EB == bb.EB &&
                old.HB == bb.HB)
            {
                return -10;
            }
            ScoreModifyRecords(old, bb);

            old.RunsA = bb.RunsA;
            old.RunsB = bb.RunsB;
            old.RA = bb.RA;
            old.HA = bb.HA;
            old.EA = bb.EA;
            old.RB = bb.RB;
            old.HB = bb.HB;
            old.EB = bb.EB;
            old.StatusText = bb.StatusText;
            old.ChangeCount++;
            old.ChangeTime = DateTime.Now;
            old.CtrlAdmin = userService.UserName;
            Update(old);
            return Commit();
        }


        public void ScoreModifyRecords(BaseballSchedules oldModel, BaseballSchedules newModel)
        {
            ScoreModifyRecord modelSmr = new ScoreModifyRecord()
            {
                ScheduleID = oldModel.GID,
                GameType = oldModel.GameType,
                RunsAOld = oldModel.RunsA,
                RunsBOld = oldModel.RunsB,
                RAOld = oldModel.RA,
                RBOld = oldModel.RB,
                HAOld = oldModel.HA,
                HBOld = oldModel.HB,
                EAOld = oldModel.EA,
                EBOld = oldModel.EB,
                StatusTextOld = newModel.GameStates == null ? null : AppData.GetStatesText(oldModel.GameStates),
                RunsANew = newModel.RunsA,
                RunsBNew = newModel.RunsB,
                RANew = newModel.RA,
                RBNew = newModel.RB,
                HANew = newModel.HA,
                HBNew = newModel.HB,
                EANew = newModel.EA,
                EBNew = newModel.EB,
                StatusTextNew = AppData.GetStatesText(newModel.GameStates),
                ModifyTime = DateTime.Now,
                ModifyUser = userService.UserName,
                IpAddr = userService.UserIP,
                ModifyItem = 0
            };
            _smrs.Add(modelSmr);
        }

        public Baseball GetSchedulesByID(int GID)
        {
            var linq = (from b in db.BaseballSchedules.Where(p => p.GID == GID && !p.IsDeleted)
                        join a in db.BaseballAlliance on b.AllianceID equals a.AllianceID
                        join tA in db.BaseballTeam on b.TeamAID equals tA.TeamID
                        join tB in db.BaseballTeam on b.TeamBID equals tB.TeamID
                        orderby b.GameTime
                        select new Baseball
                        {
                            GID = b.GID,
                            GameType = b.GameType,
                            Number = b.Number,
                            Alliance = a.AllianceName,
                            IsReschedule = b.IsReschedule,
                            ShowJS = b.ShowJS,
                            GameDate = b.GameDate,
                            GameTime = b.GameTime,
                            TeamA = tA.ShowName,
                            TeamB = tB.ShowName,
                            GameStates = b.GameStates,
                            WebID = b.WebID,
                            CtrlStates = b.CtrlStates,
                            IsDeleted = b.IsDeleted,
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


        public bool CanCtrl(int gid)
        {
            BaseballSchedules bb = QueryById(gid);
            // 已操盤時判斷是否為本人
            if (bb.CtrlStates == 1)
            {
                if (!string.IsNullOrEmpty(bb.CtrlAdmin) && bb.CtrlAdmin != userService.UserName)
                {
                    // 不是本人，不可變更
                    return false;
                }
                return true;
            }
            return true;
        }

        public int UpdateFollow(BaseballSchedules bb, int type)
        {
            BaseballSchedules old = QueryById(bb.GID);
            if (old != null)
            {
                switch (type)
                {
                    case 1:
                        return UpdateAll(bb, old);
                    case 2:
                        return UpdateBsoAndBases(bb, old);
                    case 3:
                        return UpdateRc(bb, old);
                }
            }
            return 0;
        }

        private int UpdateAll(BaseballSchedules bb, BaseballSchedules old)
        {
            if (old.RunsA == bb.RunsA &&
               old.RunsB == bb.RunsB &&
               old.RA == bb.RA &&
               old.RB == bb.RB &&
               old.StatusText == bb.StatusText &&
                old.B == bb.B &&
                old.S == bb.S &&
                old.Bases == bb.Bases &&
                old.O == bb.O &&
                old.EA == bb.EA &&
                old.HA == bb.HA &&
                old.EB == bb.EB &&
                old.HB == bb.HB &&
                old.GameStates == bb.GameStates &&
            old.TrackerText == bb.TrackerText &&
            old.Record == bb.Record)
            {
                return 10001;
            }
            ScoreModifyRecords(old, bb);
            old.B = bb.B;
            old.S = bb.S;
            old.O = bb.O;
            old.Bases = bb.Bases;
            old.RA = bb.RA;
            old.HA = bb.HA;
            old.EA = bb.EA;
            old.RB = bb.RB;
            old.HB = bb.HB;
            old.EB = bb.EB;
            old.RunsA = bb.RunsA;
            old.RunsB = bb.RunsB;
            old.GameStates = bb.GameStates;
            old.TrackerText = bb.TrackerText;
            old.Record = bb.Record;
            old.ChangeCount++;
            old.ChangeTime = DateTime.Now;

            Update(old);
            return Commit();
        }
        private int UpdateBsoAndBases(BaseballSchedules bb, BaseballSchedules old)
        {
            old.B = bb.B;
            old.S = bb.S;
            old.O = bb.O;
            old.Bases = bb.Bases;
            old.GameStates = bb.GameStates;
            old.Record = bb.Record;
            old.ChangeCount++;
            old.ChangeTime = DateTime.Now;

            Update(old);
            return Commit();
        }
        private int UpdateRc(BaseballSchedules bb, BaseballSchedules old)
        {
            old.B = bb.B;
            old.S = bb.S;
            old.O = bb.O;
            old.Bases = bb.Bases;
            old.Record = bb.Record;
            old.ChangeCount++;
            old.ChangeTime = DateTime.Now;

            Update(old);
            return Commit();
        }

        private BaseballSchedules copy(BaseballSchedules bb)
        {
            BaseballSchedules newBb = new BaseballSchedules()
            {
                GID = bb.GID,
                OrderBy = bb.OrderBy,
                AllianceID = bb.AllianceID,
                GameDate = bb.GameDate,
                GameStates = bb.GameStates,
                GameTime = bb.GameTime,
                GameType = bb.GameType,
                Number = bb.Number,
                IsDeleted = bb.IsDeleted,
                IsReschedule = bb.IsReschedule,
                ShowJS = bb.ShowJS,
                TrackerText = bb.TrackerText,
                TeamAID = bb.TeamAID,
                TeamBID = bb.TeamBID,
                CtrlStates = bb.CtrlStates,
                WebID = bb.WebID
            };
            return newBb;
        }
    }
}
