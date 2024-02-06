using MongoDBTest.Models;

namespace MongoDB.Utilities;
public class PaginationHelper
{
    public static PagedResult<T> Paginate<T>(IEnumerable<T> items, int pageNumber, int itemsPerPage)
    {
        // Set default values for pageNumber and itemsPerPage if not provided
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        itemsPerPage = itemsPerPage == 0 ? 10 : itemsPerPage;

        // Count the total number of items
        int totalItems = items.Count();

        // If no items are found, throw an exception
        if (totalItems <= 0)
            throw new Exception("No results found");

        // Calculate the total number of pages
        int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

        // Get the items for the requested page
        var pagedItems = items.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);

        // Return a PagedResult object with the pagination data and the items for the requested page
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