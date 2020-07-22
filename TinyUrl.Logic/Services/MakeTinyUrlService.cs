using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TinyUrl.DAL.Models;
using TinyUrl.DAL.Repo;

namespace TinyUrl.Logic.Services
{
    public class MakeTinyUrlService
    {
        private readonly UserRepo _userRepo;
        private readonly UrlRepo _urlRepo;

        public MakeTinyUrlService(UserRepo userRepo, UrlRepo urlRepo)
        {
            _urlRepo = urlRepo;
            _userRepo = userRepo;
        }

        public int AddUser(User user)
        {
            return _userRepo.Add(user);
        }

        public int AddUrl(Url url)
        {
            return _urlRepo.Add(url);
        }
        
        public User FindUser(int id)
        {
            return _userRepo.GetOne(id);
        }

        public Url FindUrl(int id)
        {
            return _urlRepo.GetOne(id);
        }

        public Task<User> FindFirsUserOrNullByCondition(Expression<Func<User, bool>> where)
        {
            return _userRepo.GetFirstOrDefaultAwait(where);
        }
       

        
        
    }
}