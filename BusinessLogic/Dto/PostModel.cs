using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dto
{
    public class PostModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public bool isLiked { get; set; }
    }
}
