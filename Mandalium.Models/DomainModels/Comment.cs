using Mandalium.Core.Model.Abstractions;
using Mandalium.Core.Model.Abstractions.Interfaces;
using Mandalium.Demo.Models.Enums;
using System;

namespace Mandalium.Demo.Models.DomainModels
{
    public class Comment : BaseEntityWithId<int>, IBaseEntityWithDate
    {
        public string CodeArea { get; set; }
        public bool IsApproved { get; set; }
        public PublishStatus PublishStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }



        //User

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public Comment(int blogId)
        {
            CreatedOn = DateTime.Now;
            BlogId = blogId;
            IsApproved = false;
        }
    }
}
