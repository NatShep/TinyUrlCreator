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

        public async Task AddUserAsync(User user) => await _userRepo.AddAsync(user);

        public async Task AddUrlAsync(Url url) =>await _urlRepo.AddAsync(url);

        public async Task<List<Url>> FindUrlsByUserAsync(int userId) => await _urlRepo.FindUrlsByUserAsync(userId);

        public Url FindUrlOrNullByCondition(Expression<Func<Url, bool>> where) =>
            _urlRepo.GetFirstOrDefault(where);

        public async Task<Url> FindUrlOrNullByConditionAsync(Expression<Func<Url, bool>> where) =>
           await _urlRepo.GetFirstOrDefaultAsync(where);

        public List<Url> FindSomeUrlsByCondition(Expression<Func<Url, bool>> where) => _urlRepo.GetSome(where);

        public async Task<List<Url>> FindSomeUrlsByConditionAsync(Expression<Func<Url, bool>> where) => 
            await _urlRepo.GetSomeAsync(where);

        public User FindUserOrNullByCondition(Expression<Func<User, bool>> where) =>
            _userRepo.GetFirstOrDefault(where);

        public async Task<User> FindUserOrNullByConditionAsync(Expression<Func<User, bool>> where) =>
            await _userRepo.GetFirstOrDefaultAsync(where);

        public async Task<Url> IncreaseNumberOfTransitionByOneAsync(Url url) =>
            await _urlRepo.UpdateAndReturnUrlAsync(u => u.IncreaseNumberOfTransitions(), url);

        public async Task<string> CreateTinyUrlForUserAsync(User user, string url)
        {
            var tinyUrl = TinyCreator.CreateTinyUrl(url);
            var allUrls = await _urlRepo.GetAllTinyPathsAsync();
            while (allUrls.Any(u => u == tinyUrl))
                tinyUrl = TinyCreator.CreateTinyUrl(url);
            return tinyUrl;
        }

        public async Task UpdateHistoryForUserAsync(string tinyPath, User user)
        {
            if (user.HistoryString != null)
                user.HistoryString = user.HistoryString + "," + tinyPath;
            else
                user.HistoryString = tinyPath;
            await _userRepo.UpdateHistoryAsync(user);
        }

        public async  Task<User> GetUserByUrlAsync(int urlId) =>  await _urlRepo.GetUserByUrlAsync(urlId);
        
        public async Task<List<Url>> GetHistoryListForUserAsync(User user)
        {
            var userHistory = user.History;
            var historyList = new List<Url>();
            foreach (var tinyUrl in userHistory)
            {
                var url = await _urlRepo.GetFirstOrDefaultAsync(u => u.TinyPath == tinyUrl);
                if (url == null)
                    throw new Exception("Find null links in userHistory");
                historyList.Add(url);
            }
            return historyList;
        }
    } 
}

