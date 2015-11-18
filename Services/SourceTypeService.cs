using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class SourceTypeService : RepositoryBase<SourceType>, ISourceTypeService
    {
        public SourceTypeService(IDatabaseFactory databaseFactory,IUser IUserService)
            : base(databaseFactory, IUserService)
        {
        }
       public List<SourceType> GetSourceType(string gameType,int? allianceID=null)
       {
           var linq = from sourceType in db.SourceType.Where(p => p.GameType == gameType)
                      join sourceSetting in db.SourceSetting.Where(p => allianceID == null ? true : p.AllianceID == allianceID)
                      on sourceType.GameType equals sourceSetting.GameType
                      select  sourceType;
           return linq.ToList();
        }
       public List<SourceType> GetAllSourceType(string gameType)
       {
           var linq = from team in db.BasketballTeam
                      join source in db.SourceType on team.SourceID equals source.SourceID into st
                      from sourceType in st.DefaultIfEmpty()
                      where team.GameType == gameType
                      select sourceType;
           return linq.ToList().Where(p=>p!=null&&!string.IsNullOrEmpty(p.SourceID)).Distinct(new SourceTypeComparer()).ToList();
       }

       public List<SourceType> GetBBSourceType(string gameType)
       {
           var linq = from team in db.BaseballTeam
                      join source in db.SourceType on team.SourceID equals source.SourceID into st
                      from sourceType in st.DefaultIfEmpty()
                      where team.GameType == gameType
                      select sourceType;
           return linq.ToList().Where(p => p != null && !string.IsNullOrEmpty(p.SourceID)).Distinct(new SourceTypeComparer()).ToList();
       }
    }
}
