using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobOfferBackend.Domain.Common
{
    public interface IIdentity<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }

        bool HasIdCreated { get ; }
    }
}
