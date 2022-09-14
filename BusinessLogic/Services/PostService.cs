using BusinessLogic.Interfaces;
using Domain.Context;
using Domain.Models;
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
    }
}
