using BusinessLogic.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(Post post);
        IEnumerable<PostModel> GetUserFeed(string currentUserId);
        Task<PostModel> GetPostAsync(string postId, string currentUserId);
        Task LikePostAsync(string postId, string returnUrl);
        Task DislikePostAsync(string postId, string returnUrl);
        IEnumerable<PostModel> GetLikedPosts(string currentUserId);
        Task DeletePostAsync(string postId);
    }
}
