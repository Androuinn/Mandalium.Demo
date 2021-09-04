using Mandalium.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Models.DomainModels
{
    public class Comment : BaseEntityWithId, IBaseEntityWithDate
    {
        public string CodeArea { get; set; }
        public bool IsApproved { get; set; }
        public PublishStatus PublishStatus{ get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }



        //User

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
