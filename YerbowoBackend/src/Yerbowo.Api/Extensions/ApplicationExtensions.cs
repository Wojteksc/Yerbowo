using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using System.Net;

namespace Yerbowo.Api.Extensions
{
	public static class ApplicationExtensions
	{
		public static void UseSecurityHeaders(this IApplicationBuilder app)
		{
			app.UseHsts(options => options.MaxAge(days: 365).IncludeSubdomains()); // Strict-Transport-Security: max-age=31536000; includeSubDomains
			app.UseXContentTypeOptions(); // X-Content-Type-Options: nosniff
			app.UseXfo(options => options.SameOrigin()); // X-Frame-Options: SameOrigin
			app.UseXXssProtection(options => options.EnabledWithBlockMode()); // X-XSS-Protection: 1; mode=block
			app.UseReferrerPolicy(options => options.NoReferrer()); // Referrer-Policy: no-referrer
		}

		public static void UseExceptionHandlers(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(builder =>
			{
				builder.Run(async context =>
				{
					var error = context.Features.Get<IExceptionHandlerFeature>();
					if (error != null)
					{
						context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						var result = JsonConvert.SerializeObject(new { error = error.Error.Message });
						context.Response.ContentType = "application/json";
						await context.Response.WriteAsync(result);
					}
				});
			});
		}

		public static void UseForwardedHeadersOptions(this IApplicationBuilder app)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});
		}

		public static void UseCorsOptions(this IApplicationBuilder app)
		{
			app.UseCors(builder => builder
			.WithOrigins("http://localhost:4200", "http://yerbowo.woytech.net")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials());
		}

		public static void UseEndpointsOptions(this IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapFallbackToController("Index", "Fallback");
			});
		}
	}
}
