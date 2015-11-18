using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class OSTeamService : RepositoryBase<OSTeam>, IOSTeamService
    {
        private readonly IModifyRecordService modifyRecord;
        public OSTeamService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser IUserService)
            : base(databaseFactory,IUserService)
        {
            modifyRecord = modifyRecordService;
        }
       public int UpdateTeam(OSTeam team)
        {
            OSTeam checkTeam = base.QueryByCondition(p=>p.ShowName==team.ShowName&&p.AllianceID==team.AllianceID&&p.TeamID!=team.TeamID).SingleOrDefault();
            if (checkTeam != null) return -1;
            string gameType = "BKOS";
            string Identifier = MD5Password.GenerateId();
            OSTeam oldTeam = base.QueryById(team.TeamID);
            ModifyRecord record = base.SaveModifyRecord(oldTeam, team, ActionItem.Update, CategoryItem.Team, gameType, Identifier);
            modifyRecord.Add(record);
            oldTeam.ShowName = team.ShowName;
            base.Update(oldTeam);
            return base.Commit();
        }
    }
}
