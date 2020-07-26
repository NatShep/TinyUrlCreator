using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
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

        public void AddUser(User user) => _userRepo.AddUser(user);

        public void AddUrl(Url url) => _urlRepo.AddUrl(url);

        public User FindUser(int id) => _userRepo.GetOne(id);

        public Url FindUrl(int id) => _urlRepo.GetOne(id);

        public List<Url> FindUrlsByUser(int userId) => _urlRepo.FindUrlsByUser(userId);
        
        public Url FindFirsUrlOrNullByCondition(Expression<Func<Url, bool>> where) =>
            _urlRepo.FindFirstOrDefault(where);
        

        public List<Url> FindSomeUrlsByCondition(Expression<Func<Url, bool>> where) => _urlRepo.GetSome(where);

        public User FindUserOrNullByCondition(Expression<Func<User, bool>> where) =>
            _userRepo.GetFirstOrDefault(where);
        
        public async Task<User> FindUserOrNullAsyncByCondition(Expression<Func<User, bool>> where) =>
            await _userRepo.GetFirstOrDefaultAsync(where);


        public Url FindUrlAndIncreaseNumberOfTransitionByOne(Url url)=> 
            _urlRepo.UpdateAndReturnUrl(u=> u.NumberOfTransitions = u.NumberOfTransitions+"+1", url);
        
        public string CreateTinyUrlForUser(User user, string url)
        {
            var tinyUrl = TinyCreator.CreateTinyUrl(url);
            var allUrls = _urlRepo.GetAllTinyPaths();
            while (allUrls.Any(u=> u == tinyUrl))
                tinyUrl = TinyCreator.CreateTinyUrl(url);

            return tinyUrl;
        }

        public void UpdateHistoryForUser(string tinyPath, User user) => _userRepo.UpdateHistory(user, tinyPath);
        
    }
}