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

        public IEnumerable<Post> GetUserPosts(string userId)
        {
            //get posts of users and followings
            var user = context.Users
                .Include(u => u.Posts.OrderByDescending(p=>p.Date))
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .FirstOrDefault(u => u.Id == userId);
            //var posts = user.Posts.OrderByDescending(p => p.Date);
            //followings posts later
            
            //var posts = context.Posts
            //    .Include(p => p.Likes)
            //    .Include(p => p.Comments)
            //    .Include(p=>p.User);
            return user.Posts;
        }
    }
}
