using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;
using System;

namespace JobOfferBackend.Domain.Entities
{
    public enum ApplicationState
    {
        Requested,
        Rejected,
        Accepted
    }

    public class JobApplicationProgress : BaseValueObject
    {
        public DateTime Date { get; }

        public ApplicationState State { get; }

        public string Comment { get; }

        public JobApplicationProgress(DateTime date, ApplicationState state, string comment = default)
        {
            Date = date;
            State = state;
            Comment = comment;

            Validate();
        }

        public override void Validate()
        {
            if (Date.Year == 1900)
                _errors.AppendLine(DomainErrorMessages.DATE_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
