using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Post
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
