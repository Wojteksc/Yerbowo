using Yerbowo.Api.Headers;

namespace Yerbowo.Api.Extensions;

public static class HttpResponseExtensions
{
   public static void AddPagination(this HttpResponse response, int currentPage,
   int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}