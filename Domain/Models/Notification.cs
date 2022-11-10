using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Notification
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsReceived { get; set; }

        public virtual User Reciever { get; set; }
        public string RecieverId { get; set; }
    }
}
