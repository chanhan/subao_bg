using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IBasketballAllianceService: IRepository<BasketballAlliance>
    {
        List<BasketBallAllianceTeam> GetAllianceAndTeam(string gameType);

        int UpdateAlliance(BasketballAlliance alliance, bool isAdd);

        BasketBallAllianceTeam  GetParentAlliance(string gameType, int secondAllianceID);

        string GetDataById(int id);
    }
}
