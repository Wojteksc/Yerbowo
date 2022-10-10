using Yerbowo.Application;
using Yerbowo.Application.Settings;
using Yerbowo.Infrastructure;
using Yerbowo.Infrastructure.Context;

namespace Yerbowo.Api.Extensions;

public static class ServiceExtensions
{
	public static void AddControllersOptions(this IServiceCollection services)
	{
		services.AddControllers()
			.AddJsonOptions(opt =>
			{
				opt.JsonSerializerOptions.WriteIndented = true;
				opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
				opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
	}

	public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
		services.Configure<SendGridSettings>(configuration.GetSection("SendGrid"));
	}

	public static void AddContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<YerbowoContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
	}

	public static void AddServices(this IServiceCollection services)
	{
		services.AddYerbowoInfrastructure();
		services.AddYerbowoApplication();
    }

	public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		  .AddJwtBearer(options =>
		  {
			  options.TokenValidationParameters = new TokenValidationParameters
			  {
				  ValidIssuer = configuration["Jwt:Issuer"],
				  ValidateAudience = false,
				  ValidateLifetime = true,
				  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
			  };
		  });

		services.AddAuthentication()
		.AddGoogle(opt =>
		{
			opt.ClientId = configuration["Authentication:Google:ClientId"];
			opt.ClientSecret = configuration["Authentication:Google:ClientSecret"];
		})
		.AddFacebook(opt =>
		{
			opt.ClientId = configuration["Authentication:Facebook:ClientId"];
			opt.ClientSecret = configuration["Authentication:Facebook:ClientSecret"];
		});
	}

	public static void AddAuthorizationOptions(this IServiceCollection services)
	{
		services.AddAuthorization(x => x.AddPolicy("HasAdminRole", p => p.RequireRole("admin")));
	}

	public static void AddSessionOptions(this IServiceCollection services)
	{
		services.AddDistributedMemoryCache();

		services.Configure<CookiePolicyOptions>(options =>
		{
			options.CheckConsentNeeded = context => true;
			options.MinimumSameSitePolicy = SameSiteMode.None;
		});

		services.AddSession(options =>
		{
			options.Cookie.IsEssential = true;
			options.Cookie.Name = "Cart";
			options.IdleTimeout = TimeSpan.FromDays(1);
		});
	}

	public static void AddSwagger(this IServiceCollection services)
	{
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Yerbowo API", Version = "v1" });

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{ jwtSecurityScheme, Array.Empty<string>() }
			});
        });
    }
}