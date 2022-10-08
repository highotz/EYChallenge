using MongoDB.Bson.Serialization.Attributes;

namespace EYChallenge.Utilities.SystemObjects.Entities
{
    public abstract class BaseEntity
    {
        [BsonElement("deleted")]
        public virtual bool Deleted { get; set; }
    }
}
