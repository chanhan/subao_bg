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
    public class BasketballAllianceService : RepositoryBase<BasketballAlliance>, IBasketballAllianceService
    {
        private readonly IModifyRecordService modifyRecord;
        public BasketballAllianceService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser IUserService)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = modifyRecordService;
        }
        public List<BasketBallAllianceTeam> GetAllianceAndTeam(string gameType)
        {
            var left = from team in db.BasketballTeam.Where(p => p.GameType == gameType && !p.IsDeleted)
                       join alliance in db.BasketballAlliance.Where(p => p.GameType == gameType && !p.IsDeleted)
                       on team.AllianceID equals alliance.AllianceID into al
                       from basketballalliance in al.DefaultIfEmpty()
                       join source in db.SourceType on team.SourceID equals source.SourceID into sc
                       from sourceType in sc.DefaultIfEmpty()
                       select new BasketBallAllianceTeam
                       {
                           AllianceID = basketballalliance.AllianceID,
                           AllianceName = basketballalliance.AllianceName,
                           L = team.L,
                           Lever = basketballalliance.Lever,
                           LeverOther = string.IsNullOrEmpty(basketballalliance.LeverOther) ? string.Empty : basketballalliance.LeverOther,
                           ShowName = team.ShowName,
                           T = team.T,
                           TeamID = team.TeamID,
                           TeamName = team.TeamName,
                           W = team.W,
                           SourceID = team.SourceID
                       };
            var right = from alliance in db.BasketballAlliance.Where(p => p.GameType == gameType && !p.IsDeleted)
                        join team in db.BasketballTeam.Where(p => p.GameType == gameType && !p.IsDeleted)
                        on alliance.AllianceID equals team.AllianceID into te
                        from basketballteam in te.DefaultIfEmpty()
                        join source in db.SourceType on basketballteam.SourceID equals source.SourceID into sc
                        from sourceType in sc.DefaultIfEmpty()
                        select new BasketBallAllianceTeam
                        {
                            AllianceID = alliance.AllianceID,
                            AllianceName = alliance.AllianceName,
                            L = basketballteam.L,
                            Lever = alliance.Lever,
                            LeverOther = string.IsNullOrEmpty(alliance.LeverOther) ? string.Empty : alliance.LeverOther,
                            ShowName = basketballteam.ShowName,
                            T = basketballteam.T,
                            TeamID = basketballteam.TeamID,
                            TeamName = basketballteam.TeamName,
                            W = basketballteam.W,
                            SourceID = basketballteam.SourceID
                        };
            var linq = left.Union(right);
            return linq.ToList();
        }
        private bool CheckAlliance(BasketballAlliance alliance, bool isAdd)
        {
            Expression<Func<BasketballAlliance, bool>> predicate;
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
        public int UpdateAlliance(BasketballAlliance alliance, bool isAdd)
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
                BasketballAlliance oldAlliance = base.QueryById(alliance.AllianceID);
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
        public BasketBallAllianceTeam GetParentAlliance(string gameType, int secondAllianceID)
        {
            var linq = from first in db.BasketballAlliance
                       join second in db.BasketballAlliance
                       on first.GameType equals second.GameType
                       where second.AllianceID == secondAllianceID
                       select new BasketBallAllianceTeam
                       {
                        AllianceID=first.AllianceID,
                        AllianceName=first.AllianceName,
                        LeverOther=second.LeverOther
                       };
            return linq.ToList().SingleOrDefault(p=>p.LeverOther.Split(new char[]{'*'}).ToList().Contains(p.AllianceID.ToString()));
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDataById(int id) 
        {
            string name = "";
            BasketballAlliance basket = base.QueryByCondition(m => m.AllianceID == id).SingleOrDefault();
            if(basket!=null)
                name = basket.AllianceName;
            return name; 
        }

    }
}
