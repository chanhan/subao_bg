using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IAFBAllianceService : IRepository<AFBAlliance>
    {

        List<Models.ViewModel.AFBAllianceTeam> GetAllianceAndTeam(string gameType);

        int UpdateAlliance(AFBAlliance alliance, bool isAdd);

        Models.ViewModel.AFBAllianceTeam GetParentAlliance(string gameType, int secondAllianceID);

        string GetDataById(int id);
    }
}
