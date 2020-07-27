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
    public class UserRepo: IRepo<User>
    {
        private readonly TinyUrlContext _db;

        public UserRepo() : this(new TinyUrlContext())
        {  }
        public UserRepo(TinyUrlContext context) => _db = context;
            
        public async Task<int> AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await SaveChangesAsync();
            return user.Id;
        }

        public User GetOne(int id) => _db.Users.Find(id);

        public async Task<User> GetOneAsync(int id) => await _db.Users.FindAsync(id);
        
        public User GetFirstOrDefault(Expression<Func<User, bool>> where) => _db.Users.FirstOrDefault(where);

        public async Task<User> GetFirstOrDefaultAsync(Expression<Func<User, bool>> where) => 
            await  _db.Users.FirstOrDefaultAsync(where);

        public List<User> GetSome(Expression<Func<User, bool>> where) =>
            _db.Users.Where(where).ToList();

        public async Task<List<User>> GetSomeAsync(Expression<Func<User, bool>> where) => 
            await _db.Users.Where(where).ToListAsync();


        public async Task UpdateHistoryAsync(User user) =>await SaveChangesAsync();
        
        private async  Task<int> SaveChangesAsync()
        {
            try
            {
                return await  _db.SaveChangesAsync();
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
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}