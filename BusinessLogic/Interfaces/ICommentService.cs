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
        Task AddCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetUserCommentsAsync(string currentUserId);
    }
}
