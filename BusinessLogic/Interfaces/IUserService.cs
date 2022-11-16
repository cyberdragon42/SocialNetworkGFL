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
        Task<User> GetUserByIdAsync(string userId);
        Task<ProfileModel> GetProfileAsync(string userId, string currentUserId);
        Task FollowUserAsync(string currentUserId, string userId);
        Task UnfollowUserAsync(string currentUserId, string userId);
        Task<IEnumerable<ProfileModel>> GetUserFollowsAsync(string currentUserId);
        Task<IEnumerable<ProfileModel>> GetUserFollowersAsync(string currentUserId);
        Task<IEnumerable<ProfileModel>> FindUsersAsync(string keyword, string currentUserId);
    }
}
