using IServices.Infrastructure;
using Models;
using Models.QueryModel;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IBKOSTeamService : IRepository<BKOSTeam>
    {
        List<BKOSTeam> GetBKOSTeamByCondition(BKOSTeamQuery query, int pageIndex, int pageSize, out int count);

        string GetDataById(int id);
    }
}
