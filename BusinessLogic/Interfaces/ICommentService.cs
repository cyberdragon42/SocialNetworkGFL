using BusinessLogic.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(CreateCommentDto commentDto);
        Task<IEnumerable<UsersCommentDto>> GetUserCommentsAsync(string currentUserId);
    }
}
