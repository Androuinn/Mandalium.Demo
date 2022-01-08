using System;

namespace Mandalium.Demo.Models.Dtos
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string CodeArea { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }
    }
}
