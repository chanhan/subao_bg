using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface ITennisService : IRepository<TennisSchedules>
    {
        int SaveSchedules(IList<TennisSchedules> tennis);

        List<Tennis> GetTennisSchedules(DateTime date);
    }
}
