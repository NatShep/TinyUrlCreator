using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TinyUrl.DAL.Repo
{
    interface IRepo<T>
    {
        Task<int> AddAsync(T entity);
        T GetOne(int id);
        Task<T> GetOneAsync(int id);
        T GetFirstOrDefault(Expression<Func<T, bool>> where);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where);
        List<T> GetSome(Expression<Func<T, bool>> where);
        Task<List<T>> GetSomeAsync(Expression<Func<T, bool>> where);
    }
}