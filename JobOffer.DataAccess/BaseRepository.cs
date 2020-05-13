using JobOffer.Domain.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace JobOffer.DataAccess
{
    public abstract class BaseRepository<T> where T : BaseEntity<T>
    {
        protected IMongoDatabase _database;

        public BaseRepository(IMongoDatabase database)
        {
            _database = database;
            Collection = _database.GetCollection<T>(nameof(T));
        }

        protected IMongoCollection<T> Collection { get; set; }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return Collection.Find(p => p.Id == id).SingleOrDefaultAsync();
        }

        public virtual Task<ReplaceOneResult> Upsert(T entity)
        {
            if (entity.Id == null)
            {
                entity.Id = ObjectId.GenerateNewId(DateTime.UtcNow).ToString();
            }

            return Collection.ReplaceOneAsync(p => p.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = true });
            
        }

        public virtual Task<DeleteResult> RemoveOne(T entity)
        {
            return Collection.DeleteOneAsync<T>(p => p.Id == entity.Id);
        }
    }
}
