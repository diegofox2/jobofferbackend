using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobOffer.Domain.Entities
{
    public interface IIdentity<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }

        bool HasIdCreated { get ; }
    }
}
