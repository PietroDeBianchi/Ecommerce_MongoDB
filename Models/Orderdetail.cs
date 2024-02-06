using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoDBTest.Models
{
    public class OrderDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public long orderNumber { get; set; }
        public string? productCode { get; set; }
        public double quantityOrdered { get; set; }
        public double priceEach { get; set; }
        public int orderLineNumber { get; set; }
        [BsonIgnoreIfNull]
        public Order[]? order { get; set; }
    }
}
