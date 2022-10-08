using EYChallenge.Utilities.SystemObjects.Entities;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Domain.Product.Entities
{
    public abstract class MongoAuditableEntity : MongoBaseEntity, IAuditableEntity<User, string>
    {
        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("updatedDate")]
        public DateTime UpdatedDate { get; set; }

        [IgnoreDataMember, BsonIgnore]
        public User UserWhoCreated { get; set; }

        [IgnoreDataMember, BsonIgnore]
        public User UserWhoUpdated { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("userWhoCreatedId")]
        public string UserWhoCreatedId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("userWhoUpdatedId")]
        public string UserWhoUpdatedId { get; set; }

    }
}
