using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TinyUrl.DAL.Models;

namespace TinyUrl.DAL.Repo
{
    public class UrlRepo
    {
        private readonly TinyUrlContext _db;

        public UrlRepo() : this(new TinyUrlContext())
        {
        }

        public UrlRepo(TinyUrlContext context) => _db = context;

        public int AddUrl(Url url)
        {
            _db.Urls.Add(url);
            _db.SaveChanges();
            return url.Id;
        }
        
        public List<Url> FindUrlsByUser(int userId) =>
            _db.Urls.Include(u => u.User).Where(u => u.User.Id == userId).ToList();

        public Url GetOne(int id) => _db.Urls.Find(id);

        public Url FindFirstOrDefault(Expression<Func<Url, bool>> where) =>
            _db.Urls.FirstOrDefault(where);

        public List<Url> GetSome(Expression<Func<Url, bool>> where) => _db.Urls.Where(where).ToList();

        public Url UpdateAndReturnUrl(Action<Url> act, Url url)
        {
            act.Invoke(url);
            SaveChanges();
            return url;
        }
                
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
        public List<Url> GetAll()
        {
           return _db.Urls.ToList();
        }

        public List<string> GetAllTinyPaths()
        {
            return _db.Urls.Select(u=>u.TinyPath).ToList();
        }

        public User GetUserByUrl(int urlId)
        {
            return _db.Urls.Include(u => u.User).Where(u => u.Id == urlId).Select(u=>u.User).FirstOrDefault();
        }
    }
}