using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using Common;
using IServices;
using Newtonsoft.Json;
namespace Services.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly ApplicationDb _dataContext;
        private readonly IDbSet<T> _dbset;
        private readonly IUser _user;
        protected RepositoryBase(IDatabaseFactory databaseFactory, IUser user)
        {
            _dataContext = databaseFactory.Get();
            _dbset = _dataContext.Set<T>();
            _user = user;
        }
        public ApplicationDb db { get { return _dataContext; } }
        public IUser userService { get { return _user; } }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Add(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                Add(item);
            }
        }

        /// <summary>
        ///更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T item in entities)
            {
                Update(item);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(int id)
        {
            T item = QueryById(id);
            if (item != null)
            {
                Delete(item);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        public virtual void Delete(T item)
        {
            _dbset.Remove(item);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            foreach (T item in QueryByCondition(where))
            {
                Delete(item);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            foreach (T item in entities)
            {
                Delete(item);
            }
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> QueryByCondition(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TOrder">排序字段类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public virtual IQueryable<T> QueryByConditionForPage<TOrder>(Expression<Func<T, bool>> predicate, Expression<Func<T, TOrder>> order, int pageIndex, int pageSize, out int count)
        {
            var result = QueryByCondition(predicate);
            return QueryByConditionForPage<TOrder>(result, order, pageIndex, pageSize, out count);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TOrder">排序字段类型</typeparam>
        /// <param name="result">查询结果（延迟加载）</param>
        /// <param name="order">排序条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public virtual IQueryable<T> QueryByConditionForPage<TOrder>(IQueryable<T> result, Expression<Func<T, TOrder>> order, int pageIndex, int pageSize, out int count)
        {
            count = result.Count();
            return result.OrderBy(order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> QueryAll()
        {
            return _dbset;
        }

        /// <summary>
        /// 获取单个记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T QueryById(int id)
        {
            return _dbset.Find(id);
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string JsonSerializer(T entity)
        {
            return JsonConvert.SerializeObject(entity);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public T JsonDeserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public virtual ModifyRecord SaveModifyRecord(T oldEntity, T newEntity, ActionItem action, CategoryItem category, string gameType, string Identifier)
        {
            ModifyRecord record = new ModifyRecord
            {
                ActionStatus = (int)action,
                ItemCategory = (int)category,
                ByIP = _user.UserIP,
                ByWho = _user.UserName,
                GameType = gameType,
                NewData = JsonSerializer(newEntity),
                OldData = JsonSerializer(oldEntity),
                Identifier = Identifier,
                ChangeTime = DateTime.Now
            };
            return record;
        }
        /// <summary>
        /// 提交写入数据库
        /// </summary>
        /// <returns></returns>
        public virtual int Commit()
        {
            try
            {
                return _dataContext.Commit();
            }
            catch (DbEntityValidationException e)
            {
                string str = e.EntityValidationErrors.ToList()[0].ValidationErrors.ToList()[0].ErrorMessage;
                throw;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {

                string str = ex.Message;
                throw;

            }
        }
    }
}