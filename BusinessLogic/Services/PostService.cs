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
                .Include(p => p.Comments)
                .Where(p=>p.Id==postId)
                .FirstOrDefault();

            post.Comments = post.Comments.OrderBy(c => c.Date).ToList();
            return post;
        }

        public IEnumerable<Post> GetUserFeed(string tempUserId)
        {
            //get posts of users and followings
            var user = context.Users
                .Include(u=>u.Posts)
                .Include(p=>p.Likes)
                .Include(p=>p.Comments)
                .FirstOrDefault(u => u.Id == tempUserId);
            var posts = user.Posts.OrderByDescending(p=>p.Date);
            //followings posts later
            return posts;
        }
    }
}
