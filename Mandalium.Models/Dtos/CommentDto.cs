using System;

namespace Mandalium.Demo.Models.Dtos
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
