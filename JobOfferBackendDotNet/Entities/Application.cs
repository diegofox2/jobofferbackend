using JobOffer.Domain.Base;
using System;
using System.Collections.Generic;

namespace JobOffer.Domain.Entities
{
    public class Application : BaseEntity<Application>
    {
        public DateTime Date { get; set; }

        public IEnumerable<ApplicationProgress> Progress { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<Person> Applicants { get; set; }


        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
