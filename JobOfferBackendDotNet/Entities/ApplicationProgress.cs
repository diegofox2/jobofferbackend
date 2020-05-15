using JobOffer.Domain.Base;
using JobOffer.Domain.Constants;
using System;

namespace JobOffer.Domain.Entities
{
    public enum ApplicationState
    {
        Active,
        Rejected,
        Accepted,
        Closed
    }

    public class ApplicationProgress : BaseEntity<ApplicationProgress>
    {
        public DateTime Date { get; set; }

        public Person Applicant { get; set; }

        public ApplicationState State { get; set; }

        public string Comment { get; set; }

        public override void Validate()
        {
            if (Date.Year == 1900)
                _errors.AppendLine(DomainErrorMessages.DATE_REQUIRED);

            if (Applicant == null)
                _errors.AppendLine(DomainErrorMessages.APPLICANT_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
