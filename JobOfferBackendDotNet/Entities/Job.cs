using JobOffer.Domain.Base;
using JobOffer.Domain.Constants;
using System;

namespace JobOffer.Domain.Entities
{
    public class Job : BaseValueObject
    {
        public string CompanyName { get; set; }

        public string Position { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public bool IsCurrentJob { get; set; }

        public Job(string companyName, string position, DateTime from, bool isCurrentJob, DateTime to = default)
        {
            CompanyName = companyName;
            Position = position;
            From = from;
            To = to;
            IsCurrentJob = isCurrentJob;

            Validate();
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(CompanyName))
                _errors.AppendLine(DomainErrorMessages.COMPANY_REQUIRED);

            if (string.IsNullOrEmpty(Position))
                _errors.AppendLine(DomainErrorMessages.POSITION_REQUIRED);

            if (From.Year == 1900)
                _errors.AppendLine(DomainErrorMessages.FROM_REQUIRED);

            if (!IsCurrentJob && To.Year == 1900)
                _errors.AppendLine(DomainErrorMessages.TO_REQUIRED_WHEN_ISNOT_CURRENT_JOB);

            ThrowExceptionIfErrors();
        }
    }
}
