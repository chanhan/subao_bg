using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IBKOSAllianceService : IRepository<BKOSAlliance>
    {
        List<BKOSAlliance> GetBKOSAllianceByCondition(string keyWord, int pageIndex, int pageSize,out int count);
        int AllianceDispalySet(IList<BKOSAlliance> alliance);

        string GetDataById(int id);
        int UpdateAlliance(BKOSAlliance alliance);
    }
}
