using JobOffer.Domain.Base;
using JobOffer.Domain.Constants;

namespace JobOffer.Domain.Entities
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
                _errors.Append(DomainErrorMessages.NAME_REQUIRED);

            if (string.IsNullOrEmpty(Activity))
                _errors.Append(DomainErrorMessages.ACTIVITY_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
