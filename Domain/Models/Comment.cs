using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }
    }
}
