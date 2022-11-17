using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dto
{
    public class SearchDto
    {
        public string Keyword { get; set; }
        public IEnumerable<ProfileDto> Users { get; set; }
    }
}
