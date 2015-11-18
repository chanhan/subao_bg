using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using Common;
namespace IServices
{
    public interface IBaseballSchedulesService : IRepository<BaseballSchedules>
    {
        /// <summary>
        /// 獲取比賽數據
        /// </summary>
        List<Baseball> GetSchedules(DateTime date, string gameType);

        /// <summary>
        /// id獲取
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        Baseball GetSchedulesByID(int GID);

        /// <summary>
        /// 存储设定  
        /// </summary>
        int SaveSetting(IEnumerable<Models.ViewModel.Baseball> models, SetItem si);

        /// <summary>
        /// 分数修改记录
        /// </summary>
        void ScoreModifyRecords(BaseballSchedules oldModel, BaseballSchedules newModel);

        /// <summary>
        /// 创建赛程
        /// </summary>
        /// <param name="bb"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        int CreateSchedules(BaseballSchedules bb, string gameType);

        /// <summary>
        /// 修改赛程
        /// </summary>
        /// <param name="bb">修改值</param>
        /// <returns></returns>
        int EditSchedules(BaseballSchedules bb);

        /// <summary>
        /// 修改分数
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        int EditScore(BaseballSchedules bb);

        /// <summary>
        /// 判斷是否可操盤
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        bool CanCtrl(int gid);

        /// <summary>
        /// 更新操盘数据(操盘用)
        /// </summary>
        /// <returns></returns>
        int UpdateFollow(BaseballSchedules bb, int type);
    }
}
