using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class IPAccessService : RepositoryBase<IPAccess>, IIPAccessService
    {
        private readonly IModifyRecordService _modifyRecord;

        public IPAccessService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser user)
            : base(databaseFactory, user)
        {
            _modifyRecord = modifyRecordService;
        }

        /// <summary>
        /// 查询登入ip表的所有数据
        /// </summary>
        /// <returns>返回一个结果集</returns>
        public List<IPAccess> GetIPAccessAll(int pageIndex, int pageSize, out int count)
        {
            return QueryByConditionForPage(base.QueryAll(), p => p.ModifyTime != null ? p.uid : Int32.MaxValue, pageIndex, pageSize, out  count).ToList();
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回受影响行数</returns>
        public int DeleteIPAccessById(int id)
        {
            IPAccess ipaccess = base.QueryById(id);
            ModifyRecord record = base.SaveModifyRecord(ipaccess, ipaccess, ActionItem.Delete, CategoryItem.IP, "IPAccess", MD5Password.GenerateId());
            _modifyRecord.Add(record);
            base.Delete(id);
            return base.Commit();
        }

        /// <summary>
        /// 根据id修改一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="ip">ip</param>
        /// <param name="remarks">备注</param>
        /// <param name="user">操作人</param>
        /// <returns>返回受影响行数</returns>
        public int UpdateIPAccessById(int id, string ip, string remarks, string user)
        {
            int count = 0;
            try
            {
                //查询数据库是否存在这个ip地址
                if (ExistData(ip, id)) //包含
                {
                    count = 2;
                }
                else//不包含
                {
                    if (id > 0)//主键id大于0做修改操作
                    {
                        IPAccess oldipaccess = base.QueryById(id);
                        IPAccess newipaccess = new IPAccess { uid = oldipaccess.uid, IP = ip, Remarks = remarks, ModifyUser = user, ModifyTime = DateTime.Now, AllowChange = oldipaccess.AllowChange };//新数据
                        ModifyRecord record = base.SaveModifyRecord(oldipaccess, newipaccess, ActionItem.Update, CategoryItem.IP, "IPAccess", MD5Password.GenerateId());
                        oldipaccess.IP = ip;
                        oldipaccess.Remarks = remarks;
                        oldipaccess.ModifyUser = user;
                        oldipaccess.ModifyTime = DateTime.Now;
                        oldipaccess.AllowChange = oldipaccess.AllowChange;
                        base.Update(oldipaccess);
                        _modifyRecord.Add(record);
                        base.Commit();
                        count = 1;
                    }
                    else//做添加操作
                    {

                        IPAccess ipaccess = new IPAccess { IP = ip, Remarks = remarks, ModifyUser = user, ModifyTime = DateTime.Now, AllowChange = true };
                        ModifyRecord record = base.SaveModifyRecord(null, ipaccess, ActionItem.Add, CategoryItem.IP, "IPAccess", MD5Password.GenerateId());
                        base.Add(ipaccess);
                        _modifyRecord.Add(record);
                        base.Commit();
                        count = 1;
                    }
                }
            }
            catch
            {
                return 0;
            }
            return count;
        }
        /// <summary>
        /// 根据ip地址查询一条数据
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="uid">主键id </param>
        /// <returns>返回查询结果</returns>
        public IPAccess GetDataIPAccessByIP(string ip, int uid)
        {
            var ipaccess = (from ac in db.IPAccess where ac.IP == ip && (ac.uid != uid || uid == 0) select ac).SingleOrDefault();
            return ipaccess;
        }


        /// <summary>
        /// 查询数据是否包含这个ip
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="uid">主键id</param>
        /// <returns>返回bool类型true：数据库已存在这个ip，false则不存在</returns>
        public bool ExistData(string ip, int uid)
        {
            bool exist = false;
            try
            {
                Models.IPAccess ipaccess = GetDataIPAccessByIP(ip, uid);
                if (ipaccess != null)
                {
                    exist = true;
                }
            }
            catch
            {
                throw;
            }
            return exist;
        }
    }
}
