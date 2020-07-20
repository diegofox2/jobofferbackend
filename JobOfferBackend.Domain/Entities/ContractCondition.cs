using JobOfferBackend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOfferBackend.Domain.Entities
{
    public class ContractCondition : BaseValueObject
    {
        public string StartingFrom { get; set; }

        public string WorkingDays { get; set; }

        public string KindOfContract { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
