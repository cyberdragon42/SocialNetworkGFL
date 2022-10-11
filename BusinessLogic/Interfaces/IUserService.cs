using BusinessLogic.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        public User GetUser(string userId);
        public ProfileModel GetProfile(string userId, string currentUserId);
        Task FollowUser(string userId, string id);
        Task UnfollowUser(string userId, string id);
        Task<IEnumerable<ProfileModel>> GetUserFollows(string id);
        Task<IEnumerable<ProfileModel>> GetUserFollowers(string userId);
        IEnumerable<ProfileModel> FindUsers(string keyword);
    }
}
