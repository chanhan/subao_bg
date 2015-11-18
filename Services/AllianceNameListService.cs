using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IServices;
using Models;
using Services.Infrastructure;

namespace Services
{
    public class AllianceNameListService : RepositoryBase<AllianceNameList>, IAllianceNameListService
    {
        public AllianceNameListService(IDatabaseFactory databaseFactory, IUser user)
            : base(databaseFactory, user)
        {
        }
    }
}
