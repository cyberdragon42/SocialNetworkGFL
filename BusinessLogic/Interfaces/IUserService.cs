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
        Task<ProfileDto> GetProfileAsync(string userId, string currentUserId);
        Task FollowUserAsync(string currentUserId, string userId);
        Task UnfollowUserAsync(string currentUserId, string userId);
        Task<IEnumerable<ProfileDto>> GetUserFollowsAsync(string currentUserId);
        Task<IEnumerable<ProfileDto>> GetUserFollowersAsync(string currentUserId);
        Task<IEnumerable<ProfileDto>> FindUsersAsync(string keyword, string currentUserId);
    }
}
