using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Models.Dtos
{
    public class CommentDto
    {

        public int Id { get; set; }
        public string CodeArea { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedOn { get; set; }

        public int BlogId { get; set; }

    }
}
