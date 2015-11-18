using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IServices.Infrastructure;
using Models;

namespace IServices
{
    /// <summary>
    /// 冰球联盟
    /// </summary>
    public interface IIceHockeyAllianceService : IRepository<IceHockeyAlliance>
    {
        /// <summary>
        /// 獲取聯盟列表
        /// 不属于BF冰球 !=IHBF
        /// </summary>
        List<IceHockeyAlliance> getAllianceList(string gameType);

        /// <summary>
        /// 获取联盟列表
        /// bf冰球
        /// </summary>
        List<IceHockeyAlliance> getAllianceListByIHBF(string gameType, string keyWord, int pageIndex, int pageSize,out int count);

        /// <summary>
        /// 新增聯盟
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        int CreateAlliance(IceHockeyAlliance ia);

        /// <summary>
        /// 修改联盟
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        int EditAlliance(IceHockeyAlliance ia);

        /// <summary>
        /// 保存显示设定(bf冰球)
        /// </summary>
        /// <returns></returns>
        int SaveDisplaySetting(List<IceHockeyAlliance> iaList);

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetDataById(int id);
    }
}
