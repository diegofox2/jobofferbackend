using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;

namespace JobOfferBackend.Domain.Entities
{
    public class SkillRequired : BaseValueObject
    {
        public Skill Skill { get; }

        public byte Years { get; }

        public bool IsMandatory { get; }

        public SkillRequired(Skill skill ,byte years, bool isMandatory = default)
        {
            Skill = skill;
            Years = years;
            IsMandatory = isMandatory;

            Validate();
        }


        public override void Validate()
        {
            if (Skill == null)
                _errors.AppendLine(DomainErrorMessages.SKILL_REQUIRED);

            if (Years < 1)
                _errors.AppendLine(DomainErrorMessages.YEAR_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
