using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IFootballService : IRepository<FootballSchedules>
    {
        /// <summary>
        /// 根据日期获取赛事
        /// </summary>
        /// <param name="date">日期条件</param>
        List<FootballSchedules> GetFoolballSchedules(DateTime date);

        /// <summary>
        /// 根据日期+联盟获取赛事
        /// </summary>
        /// <param name="date">日期条件</param>
        /// <param name="al">联盟条件</param>
        List<FootballSchedules> GetFoolballSchedulesByType(DateTime date,string al);

        /// <summary>
        /// 存储
        /// </summary>
        bool ScoreSave(FootballSchedules model, int modifyItem);

        /// <summary>
        /// 修改记录
        /// </summary>
        void ScoreModifyRecords(FootballSchedules odlModel, FootballSchedules newModel);

        /// <summary>
        /// 删除分数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ScoreDelete(int id);
    }
}
