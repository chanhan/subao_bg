using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface ISourceTypeService : IRepository<SourceType>
    {
        List<SourceType> GetSourceType(string gameType,int? allianceid=null);

        List<SourceType> GetAllSourceType(string gameType);

        List<SourceType> GetBBSourceType(string gameType);
    }
}
