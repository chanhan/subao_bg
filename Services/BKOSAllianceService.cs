using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Services
{
    public class BKOSAllianceService : RepositoryBase<BKOSAlliance>, IBKOSAllianceService
    {
        private readonly IModifyRecordService modifyRecord;
        public BKOSAllianceService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser IUserService)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = modifyRecordService;
        }
        public List<BKOSAlliance> GetBKOSAllianceByCondition(string keyWord, int pageIndex, int pageSize, out int count)
        {
            return QueryByConditionForPage(p => keyWord == null ? true : (p.ShowName.Contains(keyWord) || p.AllianceName.Contains(keyWord)), p => p.AlianceSortID != null ? p.AlianceSortID.Value : Int32.MaxValue, pageIndex, pageSize, out  count).ToList();
        }
        public int AllianceDispalySet(IList<BKOSAlliance> alliance)
        {
            List<BKOSAlliance> change = alliance.Where(p => p.Changed == 1).ToList();
            List<int> allianceids = change.Select(c => c.AllianceID).ToList();
            List<BKOSAlliance> oldData = base.QueryByCondition(p => allianceids.Contains(p.AllianceID)).ToList();
            string gameType = "BKOS";
            string Identifier = MD5Password.GenerateId();
            oldData.ForEach(p =>
            {
                BKOSAlliance newdata = change.SingleOrDefault(c => c.AllianceID == p.AllianceID);
                ModifyRecord record = base.SaveModifyRecord(p, newdata, ActionItem.Update, CategoryItem.Alliance, gameType, Identifier);
                modifyRecord.Add(record);
                p.Display = newdata.Display;
            });
            base.Update(oldData);
            return base.Commit();
        }
        public int UpdateAlliance(BKOSAlliance alliance)
        {
            BKOSAlliance checkAlliance = base.QueryByCondition(p=>p.ShowName==alliance.ShowName&&p.AllianceID!=alliance.AllianceID).SingleOrDefault();
            if (checkAlliance != null) return -1;
            BKOSAlliance checkAllianceSortID= base.QueryByCondition(p=>p.AlianceSortID==alliance.AlianceSortID&&p.AllianceID!=alliance.AllianceID).SingleOrDefault();
            if (checkAllianceSortID != null) return -2;
            string gameType = "BKOS";
            string Identifier = MD5Password.GenerateId();
            BKOSAlliance oldAlliance = base.QueryById(alliance.AllianceID);
            ModifyRecord record = base.SaveModifyRecord(oldAlliance, alliance, ActionItem.Update, CategoryItem.Alliance, gameType, Identifier);
            modifyRecord.Add(record);
            oldAlliance.ShowName = alliance.ShowName;
            oldAlliance.AlianceSortID = alliance.AlianceSortID;
            oldAlliance.AllianceUrl = alliance.AllianceUrl;
            base.Update(oldAlliance);
            return base.Commit();
        }
        public string GetDataById(int id)
        {
            string name = "";
            BKOSAlliance bkos = base.QueryByCondition(m => m.AllianceID == id).SingleOrDefault();
            if(bkos!=null)
                name = bkos.ShowName;
            return name;
        }
    }
}
