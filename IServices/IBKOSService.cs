using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IBKOSService : IRepository<BasketballSchedules>
    {
        List<BKOSSchedules> GetBKOSSchedules(DateTime schedulesDate);
        BKOSSchedules GetBKOSScheduleByID(int gid);
        int SaveSchedules(IList<BasketballSchedules> bkos);
        BKOSScoreModify GetModifySchedules(int gid);
        int UpdateScore(BasketballSchedules basketball, int modifyItem);
        int DeleteScore(int gid);
    }
}
