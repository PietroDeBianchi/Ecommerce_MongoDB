using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBTest.Blogic.Authentication;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("mail")]
    [Required]
    public string Email { get; set; } = null!;

    [BsonElement("password")]
    [Required]
    public string Password { get; set; } = null!;

    [BsonElement("isadmin")]
    public bool IsAdmin { get; set; }

    [BsonElement("employeeNumber")]
    public int? EmployeeNumber { get; set; }

    [BsonElement("customerNumber")]
    public int? CustomerNumber { get; set; }
}