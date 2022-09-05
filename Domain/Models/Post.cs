using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Post
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
