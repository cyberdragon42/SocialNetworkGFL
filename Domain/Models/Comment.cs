using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
