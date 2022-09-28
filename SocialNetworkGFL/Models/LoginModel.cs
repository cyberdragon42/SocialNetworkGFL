using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkGFL.Models
{
    public class LoginModel
    {
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        public string Username { get; set; }
    }
}
