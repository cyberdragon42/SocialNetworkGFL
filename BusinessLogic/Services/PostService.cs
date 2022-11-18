using AutoMapper;
using BusinessLogic.Dto;
using BusinessLogic.Interfaces;
using Domain.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class PostService: IPostService
    {
        private readonly SocialNetworkContext context;
        private readonly IMapper mapper;
        public PostService(SocialNetworkContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task CreatePostAsync(CreatePostDto postDto)
        {
            var post = mapper.Map<CreatePostDto, Post>(postDto);
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
        }

        public async Task DislikePostAsync(string postId, string currentUserId)
        {
            var like = await context.Likes
                .Where(l => l.PostId == postId && l.UserId == currentUserId)
                .FirstOrDefaultAsync();
            if (like != null)
            {
                context.Likes.Remove(like);
                await context.SaveChangesAsync();
            }
            
        }

        public async Task<ExtendedPostDto> GetPostAsync(string postId, string currentUserId)
        {
            var post = await context.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments.OrderBy(c => c.Date))
                .ThenInclude(c => c.User)
                .Where(p => p.Id == postId)
                .FirstOrDefaultAsync();

            var postModel = mapper.Map<Post, ExtendedPostDto>(post);
            postModel.IsLiked = postModel.Likes.Any(l => l.UserId == currentUserId);

            return postModel;
        }

        public IEnumerable<ExtendedPostDto> GetUserFeed(string currentUserId)
        {
            var user = context.Users
                .Include(u=>u.Followings)
                .ThenInclude(f=>f.Following)
             .Include(u => u.Posts)
             .ThenInclude(p => p.Likes)
                .Include(u => u.Posts)
             .ThenInclude(p => p.Comments)
                .FirstOrDefault(u => u.Id == currentUserId);

            var followings = context.Users.
                    Include(u => u.Followers)
                    .Include(u => u.Posts)
                        .ThenInclude(p => p.Likes)
                    .Include(u => u.Posts)
                        .ThenInclude(p => p.Comments)
                .Where(u => u.Followers.Any(f => f.FollowerId == currentUserId));


            var allPosts = user.Posts.AsEnumerable().Union(
                followings.SelectMany(f=>f.Posts
                ));

            var postModels = new List<ExtendedPostDto>();
            foreach (var p in allPosts)
            {
                var postModel = mapper.Map<Post, ExtendedPostDto>(p);
                postModel.IsLiked = p.Likes.Any(l => l.UserId == currentUserId);
                postModels.Add(postModel);
            }

            return postModels.OrderByDescending(p => p.Date);
        }

        public IEnumerable<ExtendedPostDto> GetLikedPosts(string currentUserId)
        {
            var likedPosts = context.Posts
                .Include(p => p.Likes)
                .Include(p=>p.User)
                .Include(p=>p.Comments)
                .Where(p => p.Likes.Any(l => l.UserId == currentUserId));

            var likedPostModels = new List<ExtendedPostDto>();
            foreach(var p in likedPosts)
            {
                var postModel = mapper.Map<Post, ExtendedPostDto>(p);
                postModel.IsLiked = true;
                likedPostModels.Add(postModel);
            }
            return likedPostModels;
        }

        public async Task LikePostAsync(string postId, string currentUserId)
        {
            if(!context.Likes.Any(l=>l.UserId==currentUserId&&l.PostId==postId))
            {
                var like = new Like
                {
                    UserId = currentUserId,
                    PostId = postId
                };

                await context.Likes.AddAsync(like);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeletePostAsync(string postId)
        {
            var post = context.Posts
                .First(p => p.Id == postId);
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
        }
    }
}
