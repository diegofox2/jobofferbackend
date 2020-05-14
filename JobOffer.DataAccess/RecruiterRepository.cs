using JobOffer.Domain.Entities;
using MongoDB.Driver;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JobOffer.DataAccess
{
    public class RecruiterRepository : PersonRepository
    {
        public RecruiterRepository(IMongoDatabase database) : base(database)
        {
        }

        public async new Task<Recruiter> GetByIdAsync(string id)
        {
            return (Recruiter) await base.GetByIdAsync(id);
        }

        public override Task<ReplaceOneResult> UpsertAsync(Person entity)
        {
            var recruiter = entity as Recruiter;

            recruiter.ClientCompanies.ToList().ForEach(company =>
            {
                if(!company.HasIdCreated)
                {
                    company.Id = CreateId();
                }
            });

            return base.UpsertAsync(recruiter);
        }
    }
}
