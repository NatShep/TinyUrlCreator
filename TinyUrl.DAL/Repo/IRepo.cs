using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TinyUrl.DAL.Repo
{
  public interface IRepo<T>
        {
            int Add(T entity);
            int Update(T entity);
            int Delete(T entity);
            int Delete(int id);
            T GetOne(int? id);
            List<T> GetSome(Expression<Func<T, bool>> where);
            List<T> GetAll();
        }
    }