using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Services
{
    public class OperationalRecordService : RepositoryBase<ModifyRecord>, IOperationalRecordService
    {
        public OperationalRecordService(IDatabaseFactory databaseFactory, IUser IUserService)
            : base(databaseFactory,IUserService)
        {
        }
    }
}
