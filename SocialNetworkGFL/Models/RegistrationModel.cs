using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkGFL.Models
{
    public class RegistrationModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        //public DateTime BirthDate { get; set; }
    }
}
