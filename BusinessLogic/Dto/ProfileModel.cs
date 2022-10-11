using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Dto
{
    public class ProfileModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public IEnumerable<Post> Posts { get; set; }

        public bool IsFollower { get; set; }
        public bool IsFollowed { get; set; }
    }
}
