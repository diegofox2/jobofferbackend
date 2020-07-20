using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;

namespace JobOfferBackend.Domain.Entities
{
    public enum StudyStatus
    {
        Completed,
        InCourse,
        Abandoned
    }

    public class Study : BaseValueObject
    {
        public string Institution { get; set; }

        public string Title { get; set; }

        public StudyStatus Status { get; set; }

        public Study(string institution, string title, StudyStatus status)
        {
            Institution = institution;
            Title = title;
            Status = status;

            Validate();
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Institution))
                _errors.AppendLine(DomainErrorMessages.INSTITUTION_REQUIRED);

            if (string.IsNullOrEmpty(Title))
                _errors.AppendLine(DomainErrorMessages.TITLE_REQUIRED);
            
            ThrowExceptionIfErrors();
        }
    }
}
