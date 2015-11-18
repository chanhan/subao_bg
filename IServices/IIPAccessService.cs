using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IIPAccessService : IRepository<IPAccess>
    {
        /// <summary>
        /// 查询登入ip表的所有数据
        /// </summary>
        /// <returns>返回一个结果集</returns>
        List<IPAccess> GetIPAccessAll(int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回受影响行数</returns>
        int DeleteIPAccessById(int id);

        /// <summary>
        /// 根据id修改一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="ip">ip</param>
        /// <param name="remarks">备注</param>
        /// <param name="user">操作人</param>
        /// <returns>返回受影响行数</returns>
        int UpdateIPAccessById(int id, string ip, string remarks, string user);

        /// <summary>
        /// 根据ip地址查询一条数据
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="uid">主键id</param>
        /// <returns>返回查询结果</returns>
        IPAccess GetDataIPAccessByIP(string ip,int uid);
    }
}
