using MongoDB.Bson.Serialization.Attributes;

namespace EYChallenge.Domain.Product.Entities
{
    [BsonIgnoreExtraElements]
    public class User : MongoAuditableEntity
    {
        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("lastAccess")]
        public DateTime? LastAccess { get; set; }

        [BsonElement("firstAccess")]
        public DateTime? FirstAccess { get; set; }

        [BsonElement("enabled")]
        public bool Enabled { get; set; }
    }
}
