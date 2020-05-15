using JobOffer.Domain.Base;
using System;

namespace JobOffer.Domain.Entities
{
    public class JobOffer : BaseEntity<JobOffer>
    {
        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
