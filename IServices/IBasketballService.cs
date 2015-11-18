using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IBasketballService : IRepository<BasketballSchedules>
    {

        List<Models.ViewModel.BasketBall> GetBasketballSchedules(string gameType, int? gid, DateTime date);

        int SetShowJs(IList<BasketballSchedules> shedules);

        int EditSchedule(BasketballSchedules basketball);

        int SetShow(IList<BasketballSchedules> shedules);

        BKOSScoreModify GetModifySchedules(int gid);

        int UpdateScore(BasketballSchedules basketball);
    }
}
