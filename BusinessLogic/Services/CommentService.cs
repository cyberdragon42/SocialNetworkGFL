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
    public class CommentService : ICommentService
    {
        private readonly SocialNetworkContext context;
        public CommentService(SocialNetworkContext context)
        {
            this.context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetUserCommentsAsync(string currentUserId)
        {
            var comments = await context.Comments
                .Where(c => c.UserId == currentUserId)
                .Include(c => c.User)
                .Include(c => c.Post)
                .ToListAsync();

            return comments;
        }
    }
}
