namespace Yerbowo.Application.Extensions;

public static class HttpContextAccessorExtensions
{
    public static object GetItem(this IHttpContextAccessor httpContextAccessor, string key) 
        => httpContextAccessor.HttpContext?.Items[key];

    public static void SetItem<T>(this IHttpContextAccessor httpContextAccessor, string key, T value)
        => httpContextAccessor.HttpContext?.Items.Add(key, value);
}