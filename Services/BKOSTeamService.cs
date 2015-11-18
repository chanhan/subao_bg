using IServices;
using Models;
using Models.QueryModel;
using Models.ViewModel;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class BKOSTeamService : RepositoryBase<BKOSTeam>, IBKOSTeamService
    {
        public BKOSTeamService(IDatabaseFactory databaseFactory, IUser user)
            : base(databaseFactory, user)
        {
        }
        public List<BKOSTeam> GetBKOSTeamByCondition(BKOSTeamQuery query, int pageIndex, int pageSize, out int count)
        {
            bool isNull = query == null;
            var linq = from team in db.OSTeam.Where(p => isNull ? true : (query.TeamName == null ? true : (p.ShowName.Contains(query.TeamName) || p.TeamName.Contains(query.TeamName))) && (query.AllianceID == null ? true : p.AllianceID == query.AllianceID.Value))
                       join alliance in db.OSAlliance on team.AllianceID equals alliance.AllianceID
                       orderby team.TeamID
                       select new BKOSTeam
                       {
                           AllianceName = alliance.ShowName,
                           ShowName = team.ShowName,
                           TeamName = team.TeamName,
                           TeamID = team.TeamID,
                           AllianceID = team.AllianceID
                       };
            count = linq.Count();
            return QueryByConditionForPage(linq, p => p.AllianceID, pageIndex, pageSize, out  count).ToList();
        }

        public string GetDataById(int id)
        {
            string name = "";
            OSTeam osteam = (from team in db.OSTeam where team.TeamID == id select team).SingleOrDefault();
            if (osteam != null)
                name = osteam.ShowName;
            return name;
        }
    }
}
