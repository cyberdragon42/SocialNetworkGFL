using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dto
{
    public class UsersCommentDto
    {
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}
