using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Like
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
