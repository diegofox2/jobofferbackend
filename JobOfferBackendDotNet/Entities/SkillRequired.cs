using JobOffer.Domain.Base;

namespace JobOffer.Domain.Entities
{
    public class SkillRequired : BaseEntity<SkillRequired>
    {
        public Skill Skill { get; set; }

        public byte Years { get; set; }

        public bool IsMandatory { get; set; }


        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
