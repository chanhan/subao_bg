using System.Diagnostics;

namespace Services.Infrastructure
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private ApplicationDb _dataContext;

        public ApplicationDb Get()
        {
            _dataContext = _dataContext ?? (_dataContext = new ApplicationDb());
            //_dataContext.Database.Log = log => Trace.Write(log);

            return _dataContext;
        }
    }
}