using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Infrastructure;
using IServices;
using Models;
namespace Services
{
    public class BaseballTeamService : RepositoryBase<BaseballTeam>, IBaseballTeamService
    {
        private readonly IModifyRecordService _mrs;
        public BaseballTeamService(IDatabaseFactory databaseFactory, IUser user, IModifyRecordService mrs)
            : base(databaseFactory, user)
        {
            _mrs = mrs;
        }

        public List<BaseballTeam> getTeamList(string gameType)
        {
            //查询中无法构造实体或复杂类型
            //解決辦法 先匿名 然後在用實體類
            var linq = (from t in db.BaseballTeam.Where(p => p.GameType == gameType && !p.IsDeleted)
                        join a in db.BaseballAlliance.Where(p => p.GameType == gameType && !p.IsDeleted)
                        on t.AllianceID equals a.AllianceID
                        orderby t.TeamName
                        select new
                        {
                            TeamID = t.TeamID,
                            GameType = t.GameType,
                            TeamName = t.TeamName,
                            ShowName = t.ShowName,
                            WebName = t.WebName,
                            AllianceID = t.AllianceID,
                            AllianceName = a.AllianceName,
                            W = t.W,
                            L = t.L,
                            T = t.T,
                            IsDeleted = t.IsDeleted,
                            SourceID = t.SourceID
                        }
                     ).ToList();
            return linq.Select(s => new BaseballTeam
            {
                TeamID = s.TeamID,
                GameType = s.GameType,
                TeamName = s.TeamName,
                ShowName = s.ShowName,
                WebName = s.WebName,
                AllianceID = s.AllianceID,
                AllianceName = s.AllianceName,
                W = s.W,
                L = s.L,
                T = s.T,
                IsDeleted = s.IsDeleted,
                SourceID = s.SourceID
            }).ToList();
        }

        public int CreateTeam(BaseballTeam bt)
        {
            int c = checkedModelTeam(bt, false);
            if (c < 1)
            {
                return c;
            }
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(null, bt, Common.ActionItem.Add, Common.CategoryItem.Team, bt.GameType, Common.MD5Password.GenerateId());
            Add(bt);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        public int EditTeam(BaseballTeam bt)
        {
            int c = checkedModelTeam(bt, true);
            if (c < 1)
            {
                return c;
            }
            BaseballTeam oldModel = QueryById(bt.TeamID);
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(oldModel, bt, Common.ActionItem.Update, Common.CategoryItem.Team, bt.GameType, Common.MD5Password.GenerateId());
            oldModel.TeamName = bt.TeamName;
            oldModel.ShowName = bt.ShowName;
            oldModel.AllianceID = bt.AllianceID;
            oldModel.W = bt.W;
            oldModel.L = bt.L;
            oldModel.T = bt.T;
            Update(oldModel);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        public int DeleteTeam(int teamID)
        {
            BaseballTeam oldModel = QueryById(teamID);
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(oldModel, null, Common.ActionItem.Delete, Common.CategoryItem.Team, oldModel.GameType, Common.MD5Password.GenerateId());
            oldModel.IsDeleted = true;
            Update(oldModel);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        private int checkedModelTeam(BaseballTeam bt, bool isEdit)
        {
            if (bt == null)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(bt.TeamName) || string.IsNullOrWhiteSpace(bt.ShowName))
            {
                return 0;
            }

            //檢查名稱 如果是修改不檢查自己
            if (QueryByCondition(p => p.TeamName == bt.TeamName && p.GameType == bt.GameType && !p.IsDeleted && (isEdit ? bt.TeamID != p.TeamID : true)).Count() > 0)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// 根据id获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseballTeam GetDataById(int id)
        {
            return base.QueryByCondition(m => m.TeamID == id).SingleOrDefault();
        }
    }
}
