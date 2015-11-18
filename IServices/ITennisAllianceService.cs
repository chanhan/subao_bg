using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface ITennisAllianceService : IRepository<TennisAlliance>
    {
        List<TennisAlliance> GetTennisAllianceByCondition(string keyWord, int pageIndex, int pageSize, out int count);

        int AllianceDispalySet(IList<TennisAlliance> alliance);
        /// <summary>
        /// g根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TennisAlliance GetDataById(int id);
    }
}
