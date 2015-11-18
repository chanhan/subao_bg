using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IServices;
using Models;
using Services.Infrastructure;

namespace Services
{
    public class FavoriteInfoService : RepositoryBase<FavoriteInfo>, IFavoriteInfoService
    {
        public FavoriteInfoService(IDatabaseFactory databaseFactory, IUser user)
            : base(databaseFactory, user)
        {          
        }
        
    }
}
