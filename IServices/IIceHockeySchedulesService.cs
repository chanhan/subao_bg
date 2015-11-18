using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Models.ViewModel;
using Common;
using IServices.Infrastructure;

namespace IServices
{
    /// <summary>
    /// 冰球赛事
    /// </summary>
    public interface IIceHockeySchedulesService : IRepository<IceHockeySchedules>
    {
        /// <summary>
        /// 獲取比賽數據
        /// </summary>
        List<IceHockey> GetSchedules(DateTime date, string gameType);

        /// <summary>
        /// id獲取
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        IceHockey GetSchedulesByID(int GID);

        /// <summary>
        /// 存储设定  (非BF)
        /// </summary>
        int SaveSetting(IEnumerable<Models.ViewModel.IceHockey> models, SetItem si);
        /// <summary>
        /// 存储设定  (BF冰球)
        /// </summary>
        int SaveSetting(IEnumerable<Models.ViewModel.IceHockey> models);

        /// <summary>
        /// 分数修改记录
        /// </summary>
        void ScoreModifyRecords(IceHockeySchedules oldModel, IceHockeySchedules newModel);

        /// <summary>
        /// 创建赛程
        /// </summary>
        /// <param name="bb"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        int CreateSchedules(IceHockeySchedules ih, string gameType);

        /// <summary>
        /// 修改赛程
        /// </summary>
        /// <param name="bb">修改值</param>
        /// <returns></returns>
        int EditSchedules(IceHockeySchedules ih);

        /// <summary>
        /// 修改分数(非BF)
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        int EditScore(IceHockeySchedules ih);

        /// <summary>
        /// 修改分数(BF冰球)
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        int EditScoreByIHBF(IceHockeySchedules ih);

        /// <summary>
        /// 删除分数(BF冰球中,删除编辑过的分数与状态,重新跟来源网)
        /// </summary>
        /// <param name="ih"></param>
        /// <returns></returns>
        int DeleteScore(IceHockeySchedules ih);
    }
}
