using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IBaseballAllianceService : IRepository<BaseballAlliance>
    {
        /// <summary>
        /// 獲取聯盟列表
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        List<BaseballAlliance> getAllianceList(string gameType);

        /// <summary>
        /// 新增聯盟
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        int CreateAlliance(BaseballAlliance ba);

        /// <summary>
        /// 修改联盟
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        int EditAlliance(BaseballAlliance ba);

        /// <summary>
        /// 根据id获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetDataById(int id);
    }
}
