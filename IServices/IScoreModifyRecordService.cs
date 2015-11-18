using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IScoreModifyRecordService : IRepository<ScoreModifyRecord>
    {
        List<BKOSScoreRecord> GetScoreModifyRecord(int gid,string gameType);
        List<BKOSScoreRecord> GetBasketBallScoreModifyRecord(int gid, string gameType);

        List<AFBScoreRecord> GetAFBScoreModifyRecord(int gid, string gameType);
    }
}
