using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IServices.Infrastructure;
using Models;

namespace IServices
{
    public interface IBaseballTeamService : IRepository<BaseballTeam>
    {
        List<BaseballTeam> getTeamList(string gameType);

        /// <summary>
        /// 新增隊伍
        /// </summary>
        int CreateTeam(BaseballTeam bt);

        /// <summary>
        /// 創建隊伍
        /// </summary>
        int EditTeam(BaseballTeam bt);

        /// <summary>
        /// 刪除隊伍
        /// </summary>
        int DeleteTeam(int teamID);

        /// <summary>
        /// 根据id获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseballTeam GetDataById(int id);
    }
}
