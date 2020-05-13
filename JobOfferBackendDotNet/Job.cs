using JobOffer.Domain.Base;
using System;

namespace JobOffer.Domain
{
    public class Job : BaseAgregate
    {
        public string CompanyName { get; set; }

        public string Position { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public bool IsCurrentJob { get; set; }

        public Job(string companyName, string positionn, DateTime from, bool isCurrentJob = false, DateTime to = default)
        {
            CompanyName = companyName;
            Position = positionn;
            From = from;
            To = to;
            IsCurrentJob = isCurrentJob;

            Validate();
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(CompanyName))
                _errors.Append("COMPANY_REQUIRED");

            if (string.IsNullOrEmpty(Position))
                _errors.Append("POSITION_REQUIRED");

            if (From.Year == 0)
                _errors.Append("FROM_REQUIRED");

            if (!IsCurrentJob && To.Year == 0)
                _errors.Append("TO_REQUIRED_WHEN_ISNOT_CURRENT_JOB");

            ThrowExceptionIfErrors();
        }
    }
}
