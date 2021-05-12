
using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;

namespace JobOfferBackend.Domain.Entities
{
    public class Ability : BaseValueObject
    {
        public string SkillId { get; set; }

        public byte Years { get; set; }

        public string Comment { get; set; }

        public Ability(Skill skill, byte years, string comment = default)
        {
            skill.Validate();
            SkillId = skill.Id;
            Years = years;
            Comment = comment;

            Validate();
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(SkillId))
                _errorLines.AppendLine(DomainErrorMessages.SKILL_REQUIRED);

            if (Years == 0)
                _errorLines.AppendLine(DomainErrorMessages.YEAR_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
