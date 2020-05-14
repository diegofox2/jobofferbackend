using JobOffer.Domain.Base;
using JobOffer.Domain.Constants;

namespace JobOffer.Domain.Entities
{
    public class Ability : BaseEntity<Ability>
    {
        public Skill Skill { get; set; }

        public byte Years { get; set; }

        public string Comment { get; set; }

        public Ability(Skill skill, byte years, string comment)
        {
            Skill = skill;
            Years = years;
            Comment = comment;

            Validate();
        }

        public override void Validate()
        {
            if (Skill == null)
                _errors.Append(DomainErrorMessages.SKILL_REQUIRED);

            if (Years == 0)
                _errors.Append(DomainErrorMessages.YEAR_REQUIRED);

            ThrowExceptionIfErrors();
        }
    }
}
