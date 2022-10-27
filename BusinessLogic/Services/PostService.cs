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
        public PostService(SocialNetworkContext context)
        {
            this.context = context;
        }

        public void CreatePost(Post post)
        {
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public Post GetPost(string postId)
        {
            var post = context.Posts
                .Include(p=>p.User)
                .Include(p => p.Comments.OrderBy(c=>c.Date))
                .ThenInclude(c=>c.User)
                .Where(p=>p.Id==postId)
                .FirstOrDefault();
           
            return post;
        }

        public IEnumerable<Post> GetUserPosts(string currentUserId)
        {
            var user = context.Users
                .Include(u => u.Posts)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .FirstOrDefault(u => u.Id == currentUserId);

            var followings = context.Users.
                    Include(u=>u.Followers)
                    .Include(u => u.Posts)
                    .Include(p => p.Likes)
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

            return posts.Distinct().OrderByDescending(p => p.Date);
        }
    }
}
