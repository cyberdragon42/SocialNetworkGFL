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
        User GetUser(string userId);
        Task<ProfileModel> GetProfile(string userId, string currentUserId);
        Task FollowUser(string currentUserId, string userId);
        Task UnfollowUser(string currentUserId, string userId);
        Task<IEnumerable<ProfileModel>> GetUserFollows(string currentUserId);
        Task<IEnumerable<ProfileModel>> GetUserFollowers(string currentUserId);
        Task<IEnumerable<ProfileModel>> FindUsers(string keyword);
    }
}
