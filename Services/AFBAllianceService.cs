using Common;
using IServices;
using Models;
using Models.ViewModel;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Services
{
    public class AFBAllianceService : RepositoryBase<AFBAlliance>, IAFBAllianceService
    {
        private readonly IModifyRecordService modifyRecord;
        public AFBAllianceService(IDatabaseFactory databaseFactory, IUser IUserService, IModifyRecordService IModifyRecordService)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = IModifyRecordService;
        }
        public List<AFBAllianceTeam> GetAllianceAndTeam(string gameType)
        {
            var left = from team in db.AFBTeam.Where(p => p.GameType == gameType && !p.IsDeleted)
                       join alliance in db.AFBAlliance.Where(p => p.GameType == gameType && !p.IsDeleted)
                       on team.AllianceID equals alliance.AllianceID into al
                       from basketballalliance in al.DefaultIfEmpty()
                       //join source in db.SourceType on team.SourceID equals source.SourceID into sc
                       //from sourceType in sc.DefaultIfEmpty()
                       select new AFBAllianceTeam
                       {
                           AllianceID = basketballalliance.AllianceID,
                           AllianceName = basketballalliance.AllianceName,
                           L = team.L,
                           Lever = basketballalliance.Lever,
                           LeverOther = basketballalliance.LeverOther,
                           ShowName = team.ShowName,
                           T = team.T,
                           TeamID = team.TeamID,
                           TeamName = team.TeamName,
                           W = team.W
                        //   SourceID = team.SourceID
                       };
            var right = from alliance in db.AFBAlliance.Where(p => p.GameType == gameType && !p.IsDeleted)
                        join team in db.AFBTeam.Where(p => p.GameType == gameType && !p.IsDeleted)
                        on alliance.AllianceID equals team.AllianceID into te
                        from basketballteam in te.DefaultIfEmpty()
                        //join source in db.SourceType on basketballteam.SourceID equals source.SourceID into sc
                        //from sourceType in sc.DefaultIfEmpty()
                        select new AFBAllianceTeam
                        {
                            AllianceID = alliance.AllianceID,
                            AllianceName = alliance.AllianceName,
                            L = basketballteam.L,
                            Lever = alliance.Lever,
                            LeverOther = alliance.LeverOther,
                            ShowName = basketballteam.ShowName,
                            T = basketballteam.T,
                            TeamID = basketballteam.TeamID,
                            TeamName = basketballteam.TeamName,
                            W = basketballteam.W
                          //  SourceID = basketballteam.SourceID
                        };
            var linq = left.Union(right);
            return linq.ToList();
        }
        public int UpdateAlliance(AFBAlliance alliance, bool isAdd)
        {
            if (CheckAlliance(alliance, isAdd)) return 0;
            string gameType = alliance.GameType;
            string Identifier = MD5Password.GenerateId();
            if (alliance.LeverOther == null)
            {
                alliance.LeverOther = string.Empty;
            }
            if (isAdd)
            {
                base.Add(alliance);
                base.Commit();
                ModifyRecord record = base.SaveModifyRecord(null, alliance, ActionItem.Add, CategoryItem.Alliance, gameType, Identifier);
                modifyRecord.Add(record);
            }
            else
            {
                AFBAlliance oldAlliance = base.QueryById(alliance.AllianceID);
                ModifyRecord record = base.SaveModifyRecord(oldAlliance, alliance, ActionItem.Update, CategoryItem.Alliance, gameType, Identifier);
                modifyRecord.Add(record);
                oldAlliance.AllianceName = alliance.AllianceName;
                oldAlliance.AllianceUrl = alliance.AllianceUrl;
                oldAlliance.Lever = alliance.Lever;
                oldAlliance.LeverOther = alliance.LeverOther;
                base.Update(oldAlliance);
            }
            return base.Commit();
        }
        private bool CheckAlliance(AFBAlliance alliance, bool isAdd)
        {
            Expression<Func<AFBAlliance, bool>> predicate;
            if (isAdd)
            {
                predicate = p => p.GameType == alliance.GameType && p.AllianceName == alliance.AllianceName;
            }
            else
            {
                predicate = p => p.GameType == alliance.GameType && p.AllianceName == alliance.AllianceName && p.AllianceID != alliance.AllianceID;
            }
            return base.QueryByCondition(predicate).Any();
        }
        public AFBAllianceTeam GetParentAlliance(string gameType, int secondAllianceID)
        {
            var linq = from first in db.AFBAlliance
                       join second in db.AFBAlliance
                       on first.GameType equals second.GameType
                       where second.AllianceID == secondAllianceID
                       select new AFBAllianceTeam
                       {
                           AllianceID = first.AllianceID,
                           AllianceName = first.AllianceName,
                           LeverOther = second.LeverOther
                       };
            return linq.ToList().SingleOrDefault(p => p.LeverOther.Split(new char[] { '*' }).ToList().Contains(p.AllianceID.ToString()));
        }

        public string GetDataById(int id)
        {
            string name = "";
            AFBAlliance afb = base.QueryByCondition(m => m.AllianceID == id).SingleOrDefault();
            if (afb != null)
                name = afb.AllianceName;
            return name;
        }
    }
}
