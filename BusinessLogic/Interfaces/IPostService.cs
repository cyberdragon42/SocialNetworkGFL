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
        Task CreatePostAsync(CreatePostDto postDto);
        IEnumerable<ExtendedPostDto> GetUserFeed(string currentUserId);
        Task<ExtendedPostDto> GetPostAsync(string postId, string currentUserId);
        Task LikePostAsync(string postId, string returnUrl);
        Task DislikePostAsync(string postId, string returnUrl);
        IEnumerable<ExtendedPostDto> GetLikedPosts(string currentUserId);
        Task DeletePostAsync(string postId);
    }
}
