using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using Yerbowo.Application.Services;
using Yerbowo.Application.Services.Implementations;
using Yerbowo.Application.Settings;
using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.Addresses;
using Yerbowo.Infrastructure.Data.Orders;
using Yerbowo.Infrastructure.Data.Products;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Api.Extensions
{
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

		public static void AddConfigure(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
			services.Configure<AppSettings>(configuration.GetSection("App"));
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
	}
}
