using Common;
using IServices;
using IServices.Infrastructure;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class TennisAllianceService : RepositoryBase<TennisAlliance>, ITennisAllianceService
    {
        private readonly IModifyRecordService modifyRecord;
        public TennisAllianceService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser user)
            : base(databaseFactory,user)
        {
            modifyRecord = modifyRecordService;
        }
        public List<TennisAlliance> GetTennisAllianceByCondition(string keyWord, int pageIndex, int pageSize, out int count)
        {
            return QueryByConditionForPage(p => keyWord == null ? true : (p.ShowName.Contains(keyWord) || p.AllianceName.Contains(keyWord)), p => p.AllianceID, pageIndex, pageSize, out  count).ToList();
        }
       public int AllianceDispalySet(IList<TennisAlliance> alliance) 
       {
           List<TennisAlliance> change = alliance.Where(p => p.Changed == 1).ToList();
        //   if (change.Count == 0) return 0;
           List<int> allianceids = change.Select(c => c.AllianceID).ToList();
           List<TennisAlliance> oldData = base.QueryByCondition(p => allianceids.Contains(p.AllianceID)).ToList();
           string gameType = "TN";
           string Identifier = MD5Password.GenerateId();
           oldData.ForEach(p =>
           {
               TennisAlliance newdata = change.SingleOrDefault(c => c.AllianceID == p.AllianceID);
               ModifyRecord record = base.SaveModifyRecord(p, newdata, ActionItem.Update, CategoryItem.Alliance, gameType, Identifier);
               modifyRecord.Add(record);
               p.Display = newdata.Display;
           });
           base.Update(oldData);
           return base.Commit();
        }

       public TennisAlliance GetDataById(int id)
       {
           return base.QueryByCondition(m => m.AllianceID == id).SingleOrDefault();
       }
    }
}
