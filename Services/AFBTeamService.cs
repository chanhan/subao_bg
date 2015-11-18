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
    public class AFBTeamService : RepositoryBase<AFBTeam>, IAFBTeamService
    {
        private readonly IModifyRecordService modifyRecord;
        public AFBTeamService(IDatabaseFactory databaseFactory, IUser IUserService, IModifyRecordService IModifyRecordService)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = IModifyRecordService;
        }
        public List<AFBAllianceTeam> GetAFBTeam(string gameType)
        {
            var linq = from team in db.AFBTeam.Where(p => p.GameType == gameType && !p.IsDeleted)
                       join alliance in db.AFBAlliance on team.AllianceID equals alliance.AllianceID
                       //join sourceType in db.SourceType on team.SourceID equals sourceType.SourceID into st
                       //from type in st.DefaultIfEmpty()
                       //join sourceSetting in db.SourceSetting on type.GameType equals sourceSetting.GameType into ss
                       //from set in ss.DefaultIfEmpty()
                       orderby team.ShowName
                       select new AFBAllianceTeam
                       {
                           AllianceID = team.AllianceID,
                           AllianceName = alliance.AllianceName,
                           GameType = team.GameType,
                           L = team.L,
                           ShowName = team.ShowName,
                           T = team.T,
                           W = team.W,
                           TeamID = team.TeamID,
                           TeamName = team.TeamName,
                           WebName = team.WebName
                        //   SourceID = team.SourceID
                       };
            return linq.ToList();
        }
        public int DeleteTeam(string gameType, int teamid)
        {
            string Identifier = MD5Password.GenerateId();
            AFBTeam oldTeam = base.QueryById(teamid);
            AFBTeam newTeam = new AFBTeam
            {
                AllianceID = oldTeam.AllianceID,
                GameType = oldTeam.GameType,
                IsDeleted = !oldTeam.IsDeleted,
                L = oldTeam.L,
                ShowName = oldTeam.ShowName,
                //SourceID = oldTeam.SourceID,
                T = oldTeam.T,
                TeamID = oldTeam.TeamID,
                TeamName = oldTeam.TeamName,
                W = oldTeam.W,
                WebName = oldTeam.WebName
            };
            ModifyRecord record = base.SaveModifyRecord(oldTeam, newTeam, ActionItem.Update, CategoryItem.Team, gameType, Identifier);
            modifyRecord.Add(record);
            oldTeam.IsDeleted = true;
            base.Update(oldTeam);
            return base.Commit();
        }
        public int UpdateTeam(AFBTeam team, bool isAdd)
        {
            AFBTeam checkTeam = base.QueryByCondition(p => p.TeamName == team.TeamName &&!p.IsDeleted && (isAdd ? true : p.TeamID != team.TeamID)).SingleOrDefault();
            if (checkTeam != null) return -1;       
            ModifyRecord record = new ModifyRecord();
            string Identifier = MD5Password.GenerateId();
            if (isAdd)
            {
                base.Add(team);
                base.Commit();
                record = base.SaveModifyRecord(null, team, ActionItem.Add, CategoryItem.Team, team.GameType, Identifier);
            }
            else
            {
                AFBTeam oldTeam = base.QueryById(team.TeamID);
                record = base.SaveModifyRecord(oldTeam, team, ActionItem.Update, CategoryItem.Team, team.GameType, Identifier);
                oldTeam.AllianceID = team.AllianceID;
                oldTeam.L = team.L;
                oldTeam.ShowName = team.ShowName;
                oldTeam.T = team.L;
                oldTeam.TeamName = team.TeamName;
                oldTeam.W = team.W;
                oldTeam.WebName = team.WebName;
                base.Update(oldTeam);
            }
            modifyRecord.Add(record);
            return base.Commit();
        }

        public string GetDataById(int id)
        {
            string name = "";
            AFBTeam afbteam = base.QueryByCondition(m => m.TeamID == id).SingleOrDefault();
            if(afbteam!=null)
                name = afbteam.ShowName;
            return name;
        }
    }
}
