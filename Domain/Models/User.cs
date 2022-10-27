using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
        public Guid AvatarId { get; set; }
        public string AvatarExtension { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Subscription> Followers { get; set; }
        public ICollection<Subscription> Followings { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        //public Avatar Avatar { get; set; }
        //public string AvatarId { get; set; }
    }
}
