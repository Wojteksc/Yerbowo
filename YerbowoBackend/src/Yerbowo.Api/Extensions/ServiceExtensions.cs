﻿using Yerbowo.Application.Services.Jwt;
using Yerbowo.Application.Services.PasswordValidator;
using Yerbowo.Application.Services.SendGrid;
using Yerbowo.Application.Settings;
using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.Addresses;
using Yerbowo.Infrastructure.Data.Orders;
using Yerbowo.Infrastructure.Data.Products;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Api.Extensions;

public static class ServiceExtensions
{
	public static void AddControllersOptions(this IServiceCollection services)
	{
		services.AddControllers()
			.AddNewtonsoftJson(opt =>
			{
				opt.SerializerSettings.Formatting = Formatting.Indented;
				opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
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
		services.AddTransient<YerbowoContextSeed>();

		services.AddScoped<IProductRepository, ProductRepository>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IOrderRepository, OrderRepository>();
		services.AddScoped<IAddressRepository, AddressRepository>();

		services.AddSingleton<IJwtHandler, JwtHandler>();
		services.AddSingleton<IPasswordValidator, PasswordValidator>();
		services.AddSingleton(AutoMapperConfig.Initialize());
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddSingleton<IVerificationEmailTemplateSender, VerificationEmailTemplateSender>();
		

		services.AddMediatR(AppDomain.CurrentDomain.Load("Yerbowo.Application"));
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