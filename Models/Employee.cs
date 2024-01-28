using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MongoDBTest.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int employeeNumber { get; set; }
        public string? lastName { get; set; }
        public string? firstName { get; set; }
        public string? extension { get; set; }
        public string? email { get; set; }
        public int officeCode { get; set; }
        public int? reportsTo { get; set; }
        public string? jobTitle { get; set; }
    }
}
