using JobOffer.Domain.Base;

namespace JobOffer.Domain
{
    public class Ability : BaseAgregate
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
                _errors.Append("SKILL_REQUIRED");

            if (Years == 0)
                _errors.Append("YEAR_REQUIRED");

            ThrowExceptionIfErrors();
        }
    }
}
