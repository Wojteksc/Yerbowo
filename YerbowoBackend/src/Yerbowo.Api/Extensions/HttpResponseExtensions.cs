﻿using Yerbowo.Api.Headers;

namespace Yerbowo.Api.Extensions;

public static class HttpResponseExtensions
{
   public static void AddPagination(this HttpResponse response, int currentPage,
   int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
        var camelCaseFormatter = new JsonSerializerSettings();
        camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
        response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}