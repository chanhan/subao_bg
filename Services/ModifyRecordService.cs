using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ModifyRecordService : RepositoryBase<ModifyRecord>, IModifyRecordService
    {
        public ModifyRecordService(IDatabaseFactory databaseFactory, IUser IUserService)
            : base(databaseFactory, IUserService)
        {
        }

    }
}
