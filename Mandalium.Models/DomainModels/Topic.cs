﻿using System;

namespace Mandalium.Models.DomainModels
{
    public class Topic : BaseEntityWithId, IBaseEntityWithDate
    {
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
