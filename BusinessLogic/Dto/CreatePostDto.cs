using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dto
{
    public class CreatePostDto
    {
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
