using JobOffer.Domain.Base;
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

    public class ApplicationEvolution : BaseAgregate
    {
        public DateTime Date { get; set; }

        public Person Applicant { get; set; }

        public ApplicationState State { get; set; }

        public string Comment { get; set; }

        public override void Validate()
        {
            if (Date.Year == 1900)
                _errors.Append("APPLICATON_EVOLUTION_DATE_REQUIRED");

            if (Applicant == null)
                _errors.Append("APPLICANT_REQUIRED");

            ThrowExceptionIfErrors();
        }
    }
}
