namespace MongoDBTest.Models
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T>? Items { get; set; }
    }
}
