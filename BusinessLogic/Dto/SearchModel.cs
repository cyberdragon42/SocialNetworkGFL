using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dto
{
    public class SearchModel
    {
        public string Keyword { get; set; }
        public IEnumerable<ProfileModel> Users { get; set; }
    }
}
