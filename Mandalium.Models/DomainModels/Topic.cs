using Mandalium.Core.Model.Abstractions;
using Mandalium.Core.Model.Abstractions.Interfaces;
using System;

namespace Mandalium.Models.DomainModels
{
    public class Topic : BaseEntityWithId<int>, IBaseEntityWithDate
    {
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
