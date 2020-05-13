using JobOffer.Domain.Base;

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
                _errors.Append("COMPANY_NAME_REQUIRED");

            if (string.IsNullOrEmpty(Activity))
                _errors.Append("COMPANY_ACTIVITY_REQUIRED");

            ThrowExceptionIfErrors();
        }
    }
}
