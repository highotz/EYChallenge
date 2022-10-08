using EYChallenge.Utilities.SystemObjects.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EYChallenge.Utilities.SystemObjects.Entities
{
    public abstract class MongoBaseEntity : IEntity<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public virtual string Id { get; set; }

        [BsonElement("deleted")]
        public virtual bool Deleted { get; set; }
    }
}
