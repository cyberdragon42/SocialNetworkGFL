using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Status { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Subscription> Followers { get; set; }
        public ICollection<Subscription> Followings { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
