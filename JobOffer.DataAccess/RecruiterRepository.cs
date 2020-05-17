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


        public override async Task<ReplaceOneResult> UpsertAsync(Recruiter entity)
        {
            entity.ClientCompanies.ToList().ForEach(company =>
            {
                if(!company.HasIdCreated)
                {
                    company.Id = CreateId();
                }
            });

            return await base.UpsertAsync(entity);
        }
    }
}
