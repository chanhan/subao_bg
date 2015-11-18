using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using IServices;
using Services.Infrastructure;
using Models.QueryModel;
using Models.ViewModel;

namespace Services
{
    public class IceHockeyTeamService : RepositoryBase<IceHockeyTeam>, IIceHockeyTeamService
    {
        /// <summary>
        /// 修改记录
        /// </summary>
        private readonly IModifyRecordService _mrs;
        private readonly IBKOSTeamService _osTeam; //冰球BF 和奥逊公用 model BKOSTeam

        public IceHockeyTeamService(IDatabaseFactory databaseFactory, IUser user, IModifyRecordService mrs, IBKOSTeamService osTeam)
            : base(databaseFactory, user)
        {
            _mrs = mrs;
            _osTeam = osTeam;
        }

        public List<IceHockeyTeam> getTeamList(string gameType)
        {
            //查询中无法构造实体或复杂类型
            //解決辦法 先匿名 然後在用實體類
            var linq = (from t in db.IceHockeyTeam.Where(p => p.GameType == gameType && p.Display)
                        join a in db.IceHockeyAlliance.Where(p => p.GameType == gameType && p.Display)
                        on t.AllianceID equals a.AllianceID
                        orderby t.AllianceID, t.TeamName
                        select new
                        {
                            TeamID = t.TeamID,
                            GameType = t.GameType,
                            TeamName = t.TeamName,
                            ShowName = t.ShowName,
                            WebName = t.WebName,
                            AllianceID = t.AllianceID,
                            AllianceName = a.ShowName,
                            W = t.W,
                            L = t.L,
                            T = t.T,
                            Display = t.Display
                        }
                     ).ToList();
            return linq.Select(s => new IceHockeyTeam
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
                Display = s.Display
            }).ToList();
        }

        public int CreateTeam(IceHockeyTeam it)
        {
            int c = checkedModelTeam(it, false);
            if (c < 1)
            {
                return c;
            }
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(null, it, Common.ActionItem.Add, Common.CategoryItem.Team, it.GameType, Common.MD5Password.GenerateId());
            Add(it);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        public int EditTeam(IceHockeyTeam it)
        {
            int c = checkedModelTeam(it, true);
            if (c < 1)
            {
                return c;
            }
            IceHockeyTeam oldModel = QueryById(it.TeamID);
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(oldModel, it, Common.ActionItem.Update, Common.CategoryItem.Team, it.GameType, Common.MD5Password.GenerateId());
            oldModel.TeamName = it.TeamName;
            oldModel.ShowName = it.ShowName;
            oldModel.AllianceID = it.AllianceID;
            oldModel.W = it.W;
            oldModel.L = it.L;
            oldModel.T = it.T;
            Update(oldModel);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        public int DeleteTeam(int teamID)
        {
            IceHockeyTeam oldModel = QueryById(teamID);
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(oldModel, null, Common.ActionItem.Delete, Common.CategoryItem.Team, oldModel.GameType, Common.MD5Password.GenerateId());
            oldModel.Display = false;
            Update(oldModel);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        private int checkedModelTeam(IceHockeyTeam it, bool isEdit)
        {
            if (it == null)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(it.TeamName) || string.IsNullOrWhiteSpace(it.ShowName))
            {
                return 0;
            }

            //檢查名稱 如果是修改不檢查自己
            if (QueryByCondition(p => (it.GameType=="IHBF"?p.ShowName==it.ShowName: p.TeamName == it.TeamName) && p.Display &&p.GameType == it.GameType && p.AllianceID==it.AllianceID  && (isEdit ? it.TeamID != p.TeamID : true)).Count() > 0)
            {
                return -1;
            }
            return 1;
        }

        public List<BKOSTeam> getTeamListByIHBF(string gameType, BKOSTeamQuery Query, int pageIndex, int pageSize, out int count)
        {
            //冰球BF 和奥逊公用 model BKOSTeam
            bool isNull = Query == null, isAllianceID = false, isTeamName = false;
            if (!isNull)
            {
                isAllianceID = Query.AllianceID == null;
                isTeamName = Query.TeamName == null;
            }
            var linq = (from t in db.IceHockeyTeam.Where(p => p.GameType == gameType  && (isTeamName ? true : p.ShowName.Contains(Query.TeamName) || p.TeamName.Contains(Query.TeamName)))
                        join a in db.IceHockeyAlliance.Where(p => p.GameType == gameType && (isAllianceID ? true : p.AllianceID == Query.AllianceID))
                        on t.AllianceID equals a.AllianceID
                        orderby t.TeamName
                        select new BKOSTeam
                        {
                            TeamID = t.TeamID,
                            TeamName = t.TeamName,
                            ShowName = t.ShowName,
                            AllianceID = t.AllianceID,
                            AllianceName = a.ShowName
                        }
                     );
            return _osTeam.QueryByConditionForPage(linq, (p => p.AllianceID), pageIndex, pageSize, out count).ToList();
        }

        public string GetDataById(int id)
        {
            string name = "";
            IceHockeyTeam ice = base.QueryByCondition(m => m.TeamID == id).SingleOrDefault();
            if(ice !=null)
                name= ice.ShowName;
            return name;
        }
    }
}
