
using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;

namespace JobOfferBackend.Domain.Entities
{
    public class Ability : BaseValueObject
    {
        public Skill Skill { get; set; }

        public byte Years { get; set; }

        public string Comment { get; set; }

        public Ability(Skill skill, byte years, string comment = default)
        {
            Skill = skill;
            Years = years;
            Comment = comment;

            Validate();
        }

        public override void Validate()
        {
            if (Skill == null)
                _errors.AppendLine(DomainErrorMessages.SKILL_REQUIRED);

            if (Years == 0)
                _errors.AppendLine(DomainErrorMessages.YEAR_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
