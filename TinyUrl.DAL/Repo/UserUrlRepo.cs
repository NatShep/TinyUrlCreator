using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TinyUrl.DAL.Models;
using TinyUrl.EF;

namespace TinyUrl.DAL.Repo
{
    public class UserUrlRepo
    {
        private readonly UrlContext _db;

        public UserUrlRepo(UrlContext context)
        {
            _db = context;
        }
        public void Dispose()
        {
            _db?.Dispose();
        }

        public int AddUser(User entity)
        {
            _db.Users.Add(entity);
            return SaveChanges();
        }
        
        public int AddUrl(Url entity)
        {
            _db.Urls.Add(entity);
            return SaveChanges();
        }

        public int UpdateUser(User entity)
        {
            _db.Users.Update(entity);
            return SaveChanges();
        }

        public int UpdateUrl(Url entitiy)
        {
            _db.Urls.Update(entitiy);
            return SaveChanges();
        }

    /*    public int Delete(T entity)
        {
      
            _table.Remove(entity);
            return SaveChanges();
        }

        public int Delete(int id, byte[] timeStamp)
        {
            _db.Entry(new T()
                {
                    Id = id, Timestamp = timeStamp
                })
                .State = EntityState.Deleted;
            return SaveChanges();

        }

        public T GetOne(int? id) => _table.Find(id);

        public List<T> GetSome(Expression<Func<T, bool>> where) => _table.Where(where).ToList(); 

        public virtual List<T> GetAll() => _table.ToList();

        public virtual List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending) =>
            (ascending ? _table.OrderBy(orderBy) : _table.OrderByDescending(orderBy)).ToList();

        public List<T> ExecuteQuery(string sql)
        {
            throw new NotImplementedException();
        }

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects)
        {
            throw new NotImplementedException();
        }*/

        internal int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (RetryLimitExceededException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}