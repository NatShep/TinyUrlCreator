using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TinyUrl.DAL.Models;

namespace TinyUrl.DAL.Repo
{
    public class BaseRepo<T>: IDisposable, IRepo<T> where T:EntityBase, new()
    {
        private readonly DbSet<T> _table;
        private readonly UrlContext _db;
        protected UrlContext ContextDb => _db;

        public BaseRepo() : this(new UrlContext())
        {
            
        }

        public BaseRepo(UrlContext context)
        {
            _db = context;
            _table = _db.Set<T>();
        }
        public void Dispose()
        {
            _db?.Dispose();
        }

        public int Add(T entity)
        {
            _table.Add(entity);
            return SaveChanges();
        }

        public int Update(T entity)
        {
            _table.Update(entity);
            return SaveChanges();
        }

        public int Delete(T entity)
        {
            _table.Remove(entity);
            return SaveChanges();
        }

        public int Delete(int id)
        {
            var entity = _table.FirstOrDefault(c => c.Id == id);
            _table.Remove(entity);
            return SaveChanges();
        }

        public T GetOne(int? id) => _table.Find(id);

        public List<T> GetSome(Expression<Func<T, bool>> where) => _table.Where(where).ToList();

        public async Task<T> GetFirstOrDefaultAwait(Expression<Func<T, bool>> where) =>
            await _table.FirstOrDefaultAsync(where);

      public async Task<T> GetSomeById(int? id) => await _table.FirstOrDefaultAsync(u => u.Id == id);
      
      
        public virtual List<T> GetAll() => _table.ToList();

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