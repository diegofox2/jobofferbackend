using JobOffer.Domain.Entities;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace JobOffer.DataAccess
{
    public class PersonRepository: BaseRepository<Person>
    {
        public PersonRepository(IMongoDatabase database):base(database)
        {
        }

        public override Task<ReplaceOneResult> UpsertAsync(Person entity)
        {
            entity.Studies.ToList().ForEach(study =>
            {
                if (!study.HasIdCreated)
                {
                    study.Id = CreateId();
                }
            });

            entity.JobHistory.ToList().ForEach(job =>
            {
                if (!job.HasIdCreated)
                {
                    job.Id = CreateId();
                }
            });

            entity.Abilities.ToList().ForEach(ability =>
            {
                if (!ability.HasIdCreated)
                {
                    ability.Id = CreateId();
                }
            });

            return base.UpsertAsync(entity);
        }

    }
}
