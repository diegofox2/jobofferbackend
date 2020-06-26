using JobOfferBackend.Domain.Common;
using System;
using System.Collections.Generic;

namespace JobOfferBackend.Domain.Entities
{
    public class JobApplication : BaseEntity<JobApplication>
    {
        private List<JobApplicationProgress> _applicationProgress = new List<JobApplicationProgress>();

        public DateTime Date { get; set; }

        public IEnumerable<JobApplicationProgress> Progress { get => _applicationProgress; set => _applicationProgress = (List<JobApplicationProgress>)value; }

        public bool IsActive { get; set; }

        public Person Applicant { get; set; }

        public JobApplication(Person applicant, DateTime date)
        {
            Applicant = applicant;
            IsActive = true;
            Date = date;

            _applicationProgress.Add(new JobApplicationProgress(date, ApplicationState.Requested));
        }

        public void SetAcceptedStatus()
        {
            _applicationProgress.Add(new JobApplicationProgress(DateTime.Now.Date, ApplicationState.Accepted));
        }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
