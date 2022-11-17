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
    public class CommentService : ICommentService
    {
        private readonly SocialNetworkContext context;
        private readonly IMapper mapper;
        public CommentService(SocialNetworkContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task AddCommentAsync(CreateCommentDto commentDto)
        {
            var comment = mapper.Map<CreateCommentDto, Comment>(commentDto);
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UsersCommentDto>> GetUserCommentsAsync(string currentUserId)
        {
            var comments = await context.Comments
                .Where(c => c.UserId == currentUserId)
                .Include(c => c.User)
                .Include(c => c.Post)
                .ToListAsync();

            var newComments = mapper.Map<IEnumerable<Comment>, IEnumerable<UsersCommentDto>>(comments);

            return newComments;
        }
    }
}
