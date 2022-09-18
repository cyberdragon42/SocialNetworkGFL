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
    public class CommentService : ICommentService
    {
        private readonly SocialNetworkContext context;
        public CommentService(SocialNetworkContext context)
        {
            this.context = context;
        }

        public void AddComment(Comment comment)
        {
            context.Comments.Add(comment);
            context.SaveChanges();
        }
    }
}
