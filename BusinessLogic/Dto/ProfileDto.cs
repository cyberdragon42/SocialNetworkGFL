using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Dto
{
    public class ProfileDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid AvatarId { get; set; }
        public string AvatarExtension { get; set; }
        public string Id { get; set; }
        public IEnumerable<ExtendedPostDto> Posts { get; set; }

        public bool IsFollower { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsOwnProfile { get; set; }
    }
}
