using JobOffer.Domain.Base;

namespace JobOffer.Domain.Entities
{
    public class Skill : BaseEntity<Skill>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                _errors.Append("SKILL_NAME_REQUIRED");

            ThrowExceptionIfErrors();
        }
    }
}
