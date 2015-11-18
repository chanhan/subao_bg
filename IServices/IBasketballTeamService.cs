using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IBasketballTeamService : IRepository<BasketballTeam>
    {
        List<BasketBallAllianceTeam> GetBasketBallTeam(string gameType);

        int DeleteTeam(string gameType, int teamid);

        int UpdateTeam(BasketballTeam team, bool isAdd);

        string GetDataById(int id);
    }
}
