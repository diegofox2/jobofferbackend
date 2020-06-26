using JobOfferBackend.Doman.Security.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class AccountRepository : BaseRepository<Account>
    {
        public AccountRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<Account> GetAccountAsync(string email, string password)
        {
            return await Collection.Find(item => item.Email == email && item.Password == password).SingleOrDefaultAsync();
        }

        public async Task<Account> GetByEmail(string email)
        {
            return await Collection.Find(item => item.Email == email).SingleOrDefaultAsync();
        }
    }
}
