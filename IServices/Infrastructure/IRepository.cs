using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IServices.Infrastructure
{
    public interface IRepository<T> where T : class
    {

        void Add(T entity);

        void Add(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(int id);

        void Delete(T item);

        void Delete(Expression<Func<T, bool>> where);

        void Delete(IEnumerable<T> entities);

        IQueryable<T> QueryByCondition(Expression<Func<T, bool>> predicate);
        IQueryable<T> QueryByConditionForPage<Torder>(Expression<Func<T, bool>> predicate, Expression<Func<T, Torder>> order, int pageIndex, int pageSize, out int count);
        IQueryable<T> QueryByConditionForPage<TOrder>(IQueryable<T> result, Expression<Func<T, TOrder>> order, int pageIndex, int pageSize, out int count);
      
        IQueryable<T> QueryAll();

        T QueryById(int id);

        string JsonSerializer(T entity);
        T JsonDeserialize(string jsonString);
        ModifyRecord SaveModifyRecord(T oldEntity, T newEntity, ActionItem action, CategoryItem category, string gameType, string Identifier);
        int Commit();
    }
}
