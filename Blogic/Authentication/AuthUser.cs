using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBTest.Blogic.Authentication;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("mail")]
    public string? Email { get; set; }

    [BsonElement("password")]
    public string? Password { get; set; }

    [BsonElement("isadmin")]
    public bool IsAdmin { get; set; }

    [BsonElement("employeeNumber")]
    public int? EmployeeNumber { get; set; }

    [BsonElement("customerNumber")]
    public int? CustomerNumber { get; set; }
}