namespace Yerbowo.Api.Extensions.ServiceCollectionExtensions;

public static class ControllersExtensions
{
    public static void AddControllersOptions(this IServiceCollection services)
    {
        services
            .AddControllers(opt => opt.OutputFormatters.RemoveType<StringOutputFormatter>())
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.WriteIndented = true;
                opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
    }
}