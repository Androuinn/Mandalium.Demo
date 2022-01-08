using Mandalium.Core.Model.Abstractions;
using Mandalium.Core.Model.Abstractions.Interfaces;
using Mandalium.Demo.Models.Enums;
using System;
using System.Collections.Generic;

namespace Mandalium.Demo.Models.DomainModels
{
    public class Blog : BaseEntityWithId<int>, IBaseEntityWithDate
    {
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string CodeArea { get; set; }
        public PublishStatus PublishStatus { get; set; }
        public string ImageUrl { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }


        #region Foreign Keys

        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        public ICollection<Comment> Comments{ get; set; }

        #endregion


        #region constructor
        public Blog() { }

        public Blog(int topicId)
        {
            this.TopicId = topicId;
            this.CreatedOn = DateTime.Now;
        }
        #endregion


    }
}
