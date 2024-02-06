namespace MongoDBTest.Models
{
    public class DbConfig
    {
        public string ConnectionString { get; set; } = default!;
        public DatabaseConfig Databases { get; set; } = default!;
    }

    public class DatabaseConfig
    {
        public EcommerceConfig Ecommerce { get; set; } = default!;
        public AuthenticationConfig Authentication { get; set; } = default!;
    }

    public class EcommerceConfig
    {
        public string Name { get; set; } = default!;
        public CollectionConfig Collections { get; set; } = default!;
    }

    public class AuthenticationConfig
    {
        public string Name { get; set; } = default!;
        public CollectionConfig Collections { get; set; } = default!;
    }

    public class CollectionConfig
    {
        public string Employees { get; set; } = default!;
        public string Products { get; set; } = default!;
        public string Orders { get; set; } = default!;
        public string Users { get; set; } = default!;
    }
}