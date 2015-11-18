using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class EmployeeService : RepositoryBase<Employee>, IEmployeeService
    {
        private readonly IModifyRecordService _modifyRecord;

        public IUser User
        {
            get { return userService; }
        }
        public EmployeeService(IDatabaseFactory databaseFactory, IModifyRecordService modifyRecordService, IUser user)
            : base(databaseFactory, user)
        {
            _modifyRecord = modifyRecordService;
        }

        /// <summary>
        /// 根据等级获取所有员工信息
        /// </summary>
        /// <param name="rank">等级参数</param>
        /// <returns>返回一个实体集</returns>
        public List<Employee> GetEmployeeAllByRank(string rank)
        {
            var linq = base.QueryByCondition(s => s.Rank == rank).OrderBy(s => s.Rank).OrderBy(s => s.LoginDate).OrderByDescending(s => s.LoginTime);
            return linq.ToList();
        }

        /// <summary>
        /// 根据员工帐号获取员工信息
        /// </summary>
        /// <param name="name">员工帐号</param>
        /// <returns>返回一个员工实体</returns>
        public Employee GetEmployeeByName(string name)
        {
            var linq = base.QueryByCondition(s => s.EmployeeName == name).SingleOrDefault();
            return linq;
        }

        /// <summary>
        /// 根据员工帐号删除一条信息
        /// </summary>
        /// <param name="employeename">被删除员工帐号</param>
        /// <returns>返回受影响行数</returns>
        public int Delete(string employeename)
        {
            int count = 0;
            try
            {
                var linq = GetEmployeeByName(employeename);
                ModifyRecord record = base.SaveModifyRecord(linq, null, ActionItem.Delete, CategoryItem.Account, "AcMa", MD5Password.GenerateId());
                base.Delete(linq);//删除数据
                _modifyRecord.Add(record);
                count = base.Commit();
            }
            catch
            {
                return 0;
            }
            return count;
        }


        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="emp">员工实体</param>
        /// <param name="oldname">修改之前的用户名</param>
        /// <returns></returns>
        public int UpdateEmployee(Employee emp, string oldname, string edit)
        {
            int count = 0;//操作异常
            try
            {
                if (edit == "修改")
                {
                    var olddata = GetEmployeeByName(oldname);//旧数据
                    ModifyRecord record = base.SaveModifyRecord(olddata, emp, ActionItem.Update, CategoryItem.Account, "AcMa", MD5Password.GenerateId());
                    olddata.EmployeeName = emp.EmployeeName;
                    olddata.Rank = emp.Rank;
                    olddata.Password = MD5Password.GetMD5Password(emp.Password);
                    olddata.Active = "Y";
                    if (emp.EmployeeName == oldname)
                    {
                        base.Update(olddata);
                        _modifyRecord.Add(record);
                        count = base.Commit();
                    }
                    else
                    {
                        if (GetEmployeeByName(emp.EmployeeName) != null)
                        {
                            count = 1;//该用户名已存在
                        }
                        else
                        {
                            Delete(oldname);
                            AddEmployee(emp);
                            count = 2;//操作成功
                        }
                    }
                }
                else
                {
                    if (GetEmployeeByName(emp.EmployeeName) != null)
                    {
                        count = 1;//该用户名已存在
                    }
                    else
                    {
                        count = AddEmployee(emp);
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
        /// 添加一条员工信息
        /// </summary>
        /// <param name="emp">用户实体</param>
        /// <returns></returns>
        public int AddEmployee(Employee emp)
        {
            Employee emps = new Employee();
            emps.EmployeeName = emp.EmployeeName;
            emps.Password = MD5Password.GetMD5Password(emp.Password);
            emps.Rank = emp.Rank;
            emps.Active = "Y";
            base.Add(emps);
            ModifyRecord record = base.SaveModifyRecord(null, emps, ActionItem.Add, CategoryItem.Account, "AcMa", MD5Password.GenerateId());
            _modifyRecord.Add(record);
            return base.Commit();
        }
    }
}
