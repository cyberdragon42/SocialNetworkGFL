using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dto
{
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public DateTime Date { get; set; }
    }
}
