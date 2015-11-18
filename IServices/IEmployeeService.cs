using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IEmployeeService : IRepository<Employee>
    {
        IUser User { get; }
        /// <summary>
        /// 根据等级获取所有员工信息
        /// </summary>
        /// <param name="rank">等级参数</param>
        /// <returns>返回一个实体集</returns>
        List<Employee> GetEmployeeAllByRank(string rank);

        /// <summary>
        /// 根据员工帐号删除一条员工信息
        /// </summary>
        /// <param name="employeename">员工帐号</param>
        /// <returns></returns>
        int Delete(string employeename);

        /// <summary>
        /// 根据员工帐号查询一条员工信息
        /// </summary>
        /// <param name="name">员工帐号</param>
        /// <returns></returns>
        Employee GetEmployeeByName(string name);

        /// <summary>
        /// 修改一条用户信息
        /// </summary>
        /// <param name="emp">用户实体</param>
        /// <param name="oldname">修改之前的用户名</param>
        /// <returns></returns>
        int UpdateEmployee(Employee emp, string oldname, string edit);

        /// <summary>
        /// 添加一条用户信息
        /// </summary>
        /// <param name="emp">用户实体</param>
        /// <returns></returns>
        int AddEmployee(Employee emp);
    }
}
