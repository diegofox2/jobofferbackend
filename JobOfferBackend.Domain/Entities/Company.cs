using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;

namespace JobOfferBackend.Domain.Entities
{
    public class Company : BaseEntity<Company>
    {
        public string Name { get; set; }

        public string Activity { get; set; }

        public Company(string name, string activity)
        {
            Name = name;
            Activity = activity;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                _errors.AppendLine(DomainErrorMessages.NAME_REQUIRED);

            if (string.IsNullOrEmpty(Activity))
                _errors.AppendLine(DomainErrorMessages.ACTIVITY_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
