using MongoDB.Bson.Serialization.Attributes;

namespace EYChallenge.Domain.Product.Entities
{
    public abstract class IntegrationAuditableEntity : MongoAuditableEntity
    {
        [BsonElement("integrationCreatedDate")]
        public DateTime IntegrationCreatedDate { get; set; }

        [BsonElement("integrationUpdatedDate")]
        public DateTime IntegrationUpdatedDate { get; set; }

        [BsonElement("reprocessedDate")]
        public DateTime ReprocessedDate { get; set; }

    }
}
