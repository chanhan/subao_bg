using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IAFBTeamService : IRepository<AFBTeam>
    {
        List<Models.ViewModel.AFBAllianceTeam> GetAFBTeam(string gameType);

        int DeleteTeam(string gameType, int teamid);

        int UpdateTeam(AFBTeam team, bool isAdd);
        string GetDataById(int id);
    }
}
