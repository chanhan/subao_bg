using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IOSTeamService : IRepository<OSTeam>
    {

        int UpdateTeam(OSTeam team);
    }
}
