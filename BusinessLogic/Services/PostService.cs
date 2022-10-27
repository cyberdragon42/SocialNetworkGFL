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

        public void CreatePost(Post post)
        {
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public void DislikePost(string postId, string currentUserId)
        {
            var like = context.Likes
                .Where(l => l.PostId == postId && l.UserId == currentUserId)
                .FirstOrDefault();
            context.Likes.Remove(like);
            context.SaveChanges();
        }

        public PostModel GetPost(string postId, string currentUserId)
        {
            var post = context.Posts
                .Include(p=>p.User)
                .Include(p=>p.Likes)
                .Include(p => p.Comments.OrderBy(c=>c.Date))
                .ThenInclude(c=>c.User)
                .Where(p=>p.Id==postId)
                .FirstOrDefault();

            var postModel = mapper.Map<Post, PostModel>(post);
            postModel.isLiked = postModel.Likes.Any(l => l.UserId == currentUserId);

            return postModel;
        }

        public IEnumerable<PostModel> GetUserPosts(string currentUserId)
        {
            var user = context.Users
                .Include(u => u.Posts)
                .ThenInclude(p => p.Likes)
                .Include(p => p.Comments)
                .FirstOrDefault(u => u.Id == currentUserId);

            var followings = context.Users.
                    Include(u=>u.Followers)
                    .Include(u => u.Posts)
                    .ThenInclude(p => p.Likes)
                    .Include(p => p.Comments)
                .Where(u => u.Followers.Any(f=>f.FollowerId== currentUserId));

            var posts = user.Posts.ToList();

            if (followings != null)
            {
                foreach (var f in followings)
                {
                    posts = posts.Concat(f.Posts).ToList();
                }
            }

            var postModels = new List<PostModel>();
            foreach(var p in posts)
            {
                var postModel = mapper.Map<Post, PostModel>(p);
                postModel.isLiked = p.Likes.Any(l => l.UserId == currentUserId);
                postModels.Add(postModel);
            }

            return postModels.Distinct().OrderByDescending(p => p.Date);
        }

        public void LikePost(string postId, string currentUserId)
        {
            var like = new Like
            {
                UserId = currentUserId,
                PostId = postId
            };

            context.Likes.Add(like);
            context.SaveChanges();
        }
    }
}
