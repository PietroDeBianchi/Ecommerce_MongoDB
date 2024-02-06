using MongoDBTest.Models;

namespace MongoDB.Utilities;
public class PaginationHelper
{
    public static PagedResult<T> Paginate<T>(IEnumerable<T> items, int pageNumber, int itemsPerPage)
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        itemsPerPage = itemsPerPage == 0 ? 10 : itemsPerPage;

        int totalItems = items.Count();

        if (totalItems <= 0)
            throw new Exception("No results found");

        int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

        var pagedItems = items.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);

        return new PagedResult<T>
        {
            PageNumber = pageNumber,
            ItemsPerPage = itemsPerPage,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Items = pagedItems
        };
    }
}