using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Subscription
    {
        public string FollowerId { get; set; }
        public virtual User Follower { get; set; }

        public string FollowingId { get; set; }
        public virtual User Following { get; set; }
    }
}
