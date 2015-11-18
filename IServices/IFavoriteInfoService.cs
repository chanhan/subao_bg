using IServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;

namespace IServices
{
    public interface IFavoriteInfoService : IRepository<FavoriteInfo>
    {
    }
}
