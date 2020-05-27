using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;

namespace JobOfferBackend.Domain.Entities
{
    public class Skill : BaseEntity<Skill>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _errors.AppendLine(DomainErrorMessages.NAME_REQUIRED);
            }

            ThrowExceptionIfErrors();
        }
    }
}
