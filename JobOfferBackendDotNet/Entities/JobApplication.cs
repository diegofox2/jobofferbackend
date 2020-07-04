using JobOfferBackend.Domain.Common;
using System;
using System.Collections.Generic;

namespace JobOfferBackend.Domain.Entities
{
    public class JobApplication : BaseValueObject
    {
        private List<JobApplicationProgress> _applicationProgress = new List<JobApplicationProgress>();

        public DateTime Date { get; set; }

        public IEnumerable<JobApplicationProgress> Progress { get => _applicationProgress; set => _applicationProgress = (List<JobApplicationProgress>)value; }

        /// <summary>
        /// The person applitant to the job
        /// </summary>
        public string PersonId { get; set; }

        public JobApplication(string personId, DateTime date)
        {
            PersonId = personId;
            Date = date;

            _applicationProgress.Add(new JobApplicationProgress(date, ApplicationState.Recieved));
        }

        public void SetStatusRequested()
        {
            _applicationProgress.Add(new JobApplicationProgress(DateTime.Now.Date, ApplicationState.Requested));
        }

        public void SetStatusAccepted()
        {
            _applicationProgress.Add(new JobApplicationProgress(DateTime.Now.Date, ApplicationState.Accepted));
        }

        public void SetStatusOffered()
        {
            _applicationProgress.Add(new JobApplicationProgress(DateTime.Now.Date, ApplicationState.Offered));
        }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
