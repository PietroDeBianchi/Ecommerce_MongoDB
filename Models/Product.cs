using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBTest.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string? productCode { get; set; }
        public string? productName { get; set; }
        public string? productLine { get; set; }
        public string? productScale { get; set; }
        public string? productVendor { get; set; }
        public string? productDescription { get; set; }
        public int quantityInStock { get; set; }
        public double buyPrice { get; set; }
        public double MSRP { get; set; }
        [BsonIgnoreIfNull]
        public OrderDetail[]? orderDetails { get; set; }
    }
}
