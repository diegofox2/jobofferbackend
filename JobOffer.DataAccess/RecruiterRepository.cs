using JobOffer.Domain.Entities;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace JobOffer.DataAccess
{
    public class RecruiterRepository : BaseRepository<Recruiter>
    {
        public RecruiterRepository(IMongoDatabase database) : base(database)
        {
        }

        public override Task<ReplaceOneResult> UpsertAsync(Recruiter entity)
        {
            entity.Studies.ToList().ForEach(study =>
            {
                if(!study.HasIdCreated)
                {
                    study.Id = CreateId();
                }
            });

            entity.JobHistory.ToList().ForEach(job =>
            {
                if(!job.HasIdCreated)
                {
                    job.Id = CreateId();
                }
            });

            entity.Abilities.ToList().ForEach(ability =>
            {
                if(!ability.HasIdCreated)
                {
                    ability.Id = CreateId();
                }
            });

            entity.ClientCompanies.ToList().ForEach(company =>
            {
                if(!company.HasIdCreated)
                {
                    company.Id = CreateId();
                }
            });

            return base.UpsertAsync(entity);
        }
    }
}
