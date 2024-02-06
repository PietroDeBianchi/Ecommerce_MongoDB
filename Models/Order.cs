using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MongoDBTest.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public long orderNumber { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime requiredDate { get; set; }
        public DateTime? shippedDate { get; set; }
        public string? status { get; set; }
        public string? comments { get; set; }
        public int customerNumber { get; set; }
        [BsonIgnoreIfNull]
        public OrderDetail[]? orderDetails { get; set; }
    }
}
