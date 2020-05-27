using JobOfferBackend.Domain.Common;
using System;
using System.Collections.Generic;

namespace JobOfferBackend.Domain.Entities
{
    public class JobApplication : BaseEntity<JobApplication>
    {
        private List<JobApplicationProgress> _applicationProgress = new List<JobApplicationProgress>();

        public DateTime Date { get;}

        public IEnumerable<JobApplicationProgress> Progress { get => _applicationProgress; set => _applicationProgress = (List<JobApplicationProgress>)value; }

        public bool IsActive { get; set; }

        public Person Applicant { get; }

        public JobApplication(Person applicant, DateTime date)
        {
            Applicant = applicant;
            IsActive = true;
            Date = date;

            _applicationProgress.Add(new JobApplicationProgress(applicant, date, ApplicationState.Requested));
        }

        public void SetAcceptedStatus()
        {
            _applicationProgress.Add(new JobApplicationProgress(Applicant, DateTime.Now.Date, ApplicationState.Accepted));
        }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
