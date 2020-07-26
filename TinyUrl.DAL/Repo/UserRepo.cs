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
    public class UserRepo
    {
        private readonly TinyUrlContext _db;

        public UserRepo() : this(new TinyUrlContext())
        {  }
        public UserRepo(TinyUrlContext context)
        {
            _db = context;
            
        }

        public void AddUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User GetOne(in int id) => _db.Users.Find(id);

        public Task<User> GetFirstOrDefaultAsync(Expression<Func<User, bool>> where) => _db.Users.FirstOrDefaultAsync(where);
        
        public User GetFirstOrDefault(Expression<Func<User, bool>> where) => _db.Users.FirstOrDefault(where);

        public List<User> GetSome(Expression<Func<User, bool>> where) => _db.Users.Where(where).ToList();


        public void UpdateHistory(User user) => SaveChanges();
        
        private int SaveChanges()
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