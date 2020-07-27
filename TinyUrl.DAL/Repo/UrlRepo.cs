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
    public class UrlRepo : IRepo<Url>
    {
        private readonly TinyUrlContext _db;

        public UrlRepo() : this(new TinyUrlContext())
        {
        }

        public UrlRepo(TinyUrlContext context) => _db = context;

        public async Task<int> AddAsync(Url url)
        {
            await _db.Urls.AddAsync(url);
            await SaveChangesAsync();
            return url.Id;
        }

        public Url GetOne(int id) => _db.Urls.Find(id);

        public async Task<Url> GetOneAsync(int id) => await _db.Urls.FindAsync(id);

        public Url GetFirstOrDefault(Expression<Func<Url, bool>> where) =>
            _db.Urls.FirstOrDefault(where);

        public async Task<Url> GetFirstOrDefaultAsync(Expression<Func<Url, bool>> where) =>
            _db.Urls.FirstOrDefault(where);

        public List<Url> GetSome(Expression<Func<Url, bool>> where) => _db.Urls.Where(where).ToList();

        public async Task<List<Url>> GetSomeAsync(Expression<Func<Url, bool>> where) =>
            await _db.Urls.Where(where).ToListAsync();

        public List<Url> FindUrlsByUser(int userId) =>
            _db.Urls.Include(u => u.User).Where(u => u.User.Id == userId).ToList();

        public async Task<List<Url>> FindUrlsByUserAsync(int userId) =>
            await _db.Urls.Include(u => u.User).Where(u => u.User.Id == userId).ToListAsync();


        public async  Task<Url> UpdateAndReturnUrlAsync(Action<Url> act, Url url)
        {
            act.Invoke(url);
            await SaveChangesAsync();
            return url;
        }

        private async  Task<int> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (RetryLimitExceededException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Url> GetAll()=> _db.Urls.ToList();
        
        public async Task<List<string>> GetAllTinyPathsAsync() => 
            await _db.Urls.Select(u => u.TinyPath).ToListAsync();
        
        public async  Task<User> GetUserByUrlAsync(int urlId) => 
            await _db.Urls
                .Include(u => u.User)
                .Where(u => u.Id == urlId)
                .Select(u => u.User)
                .FirstOrDefaultAsync();
    }
}