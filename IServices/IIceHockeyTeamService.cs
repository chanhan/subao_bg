using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using IServices.Infrastructure;
using Models.QueryModel;

namespace IServices
{
    /// <summary>
    /// 冰球队伍
    /// </summary>
    public interface IIceHockeyTeamService : IRepository<IceHockeyTeam>
    {
        /// <summary>
        /// 获取队伍列表
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        List<IceHockeyTeam> getTeamList(string gameType);
        /// <summary>
        /// 获取队伍列表(BF)
        /// 和奥逊公用一个ViewModel
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        List<Models.ViewModel.BKOSTeam> getTeamListByIHBF(string gameType, BKOSTeamQuery keyWord, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 新增隊伍
        /// </summary>
        int CreateTeam(IceHockeyTeam it);
        /// <summary>
        /// 修改隊伍
        /// </summary>
        int EditTeam(IceHockeyTeam it);
        /// <summary>
        /// 刪除隊伍
        /// </summary>
        int DeleteTeam(int teamID);

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetDataById(int id);
    }
}
