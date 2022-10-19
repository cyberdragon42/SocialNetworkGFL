using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Avatar
    {
        public string Id { get; set; }
        public Guid Name { get; set; }
        public string Extention { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
