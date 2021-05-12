using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;
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
        /// This is the Person Id
        /// </summary>
        public string ApplicantId { get; set; }

        public JobApplication(string personId, DateTime date)
        {
            ApplicantId = personId;
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
            if (string.IsNullOrEmpty(ApplicantId))
                _errorLines.AppendLine(DomainErrorMessages.APPLICANT_REQUIRED);

            if (Date.Year == 1900)
                _errorLines.AppendLine(DomainErrorMessages.DATE_REQUIRED);
        }
    }
}
