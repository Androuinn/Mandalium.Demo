using Mandalium.Models.Enums;

namespace Mandalium.Models.DomainModels
{
    public class Blog : BaseEntityWithId
    {
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string CodeArea { get; set; }
        public PublishStatus PublishStatus { get; set; }
        public string ImageUrl { get; set; }




        #region Foreign Keys

        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        #endregion

    }
}
