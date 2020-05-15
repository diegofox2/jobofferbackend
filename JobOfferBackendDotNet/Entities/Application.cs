using JobOffer.Domain.Base;
using System;
using System.Collections.Generic;

namespace JobOffer.Domain.Entities
{
    public class Application : BaseEntity<Application>
    {
        private List<ApplicationProgress> _applicationProgress = new List<ApplicationProgress>();

        public DateTime Date { get;}

        public IEnumerable<ApplicationProgress> Progress { get => _applicationProgress; set => _applicationProgress = (List<ApplicationProgress>)value; }

        public bool IsActive { get; set; }

        public Person Applicant { get; }

        public Application(Person applicant, DateTime date)
        {
            Applicant = applicant;
            IsActive = true;
            Date = date;

            _applicationProgress.Add(new ApplicationProgress(applicant, date, ApplicationState.Requested));
        }


        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
