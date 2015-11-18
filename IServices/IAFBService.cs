using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IAFBService : IRepository<AFBSchedules>
    {

        List<AFB> GetAFBSchedules(string gameType, int? gid, DateTime date);

        int SetShowJs(IList<AFBSchedules> shedules);

        int SetShow(IList<AFBSchedules> shedules);

        int EditSchedule(AFBSchedules basketball);

        AFBScoreModify GetModifySchedules(int gid);

        int UpdateScore(AFBSchedules basketball);
    }
}
