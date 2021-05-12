using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;

namespace JobOfferBackend.Domain.Entities
{
    public class SkillRequired : BaseValueObject
    {
        public string SkillId { get; set; }

        public byte Years { get; set; }

        public bool IsMandatory { get; set; }

        public SkillRequired(Skill skill ,byte years, bool isMandatory = default)
        {
            skill.Validate();

            SkillId = skill.Id;
            Years = years;
            IsMandatory = isMandatory;

            Validate();
        }


        public override void Validate()
        {
            if (string.IsNullOrEmpty(SkillId))
                _errorLines.AppendLine(DomainErrorMessages.SKILL_REQUIRED);

            if (Years < 1)
                _errorLines.AppendLine(DomainErrorMessages.YEAR_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
