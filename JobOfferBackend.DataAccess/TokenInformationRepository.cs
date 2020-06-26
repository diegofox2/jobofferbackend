using JobOfferBackend.Doman.Security.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class TokenInformationRepository : BaseRepository<TokenInformation>
    {
        public TokenInformationRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<bool> CheckValidToken(string token, string accountId)
        {
            return await Collection.Find(item => item.Id == token && item.AccountId == accountId).CountDocumentsAsync() > 0;
        }
    }
}
