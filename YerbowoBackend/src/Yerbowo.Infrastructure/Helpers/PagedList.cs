﻿namespace Yerbowo.Infrastructure.Helpers;

public class PagedList<T> : List<T>
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public int TotalPages { get; set; }
	public int TotalCount { get; set; }

	public PagedList(List<T> items, int count, int pageNumber, int pageSize)
	{
		PageSize = pageSize;
		PageNumber = pageNumber;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);
		TotalCount = count;
		AddRange(items);
	}

	public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
	{
		int count = await source.CountAsync();
		var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
		return new PagedList<T>(items, count, pageNumber, pageSize);
	}
}