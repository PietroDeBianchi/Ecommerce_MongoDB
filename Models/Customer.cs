using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MongoDBTest.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int customerNumber { get; set; }
        public string? customerName { get; set; }
        public string? contactLastName { get; set; }
        public string? contactFirstName { get; set; }
        public string? phone { get; set; }
        public string? addressLine1 { get; set; }
        public string? addressLine2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postalCode { get; set; }
        public string? country { get; set; }
        public int salesRepEmployeeNumber { get; set; }
        public decimal creditLimit { get; set; }
    }
}