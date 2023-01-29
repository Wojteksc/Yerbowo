﻿namespace Yerbowo.Api.Extensions;

public static class ApplicationExtensions
{
	public static void UseSecurityHeaders(this IApplicationBuilder app)
	{
		app.UseHsts(options => options.MaxAge(days: 365).IncludeSubdomains()); // Strict-Transport-Security: max-age=31536000; includeSubDomains
		app.UseXContentTypeOptions(); // X-Content-Type-Options: nosniff
		app.UseXfo(options => options.SameOrigin()); // X-Frame-Options: SameOrigin
		app.UseXXssProtection(options => options.EnabledWithBlockMode()); // X-XSS-Protection: 1; mode=block
		app.UseReferrerPolicy(options => options.StrictOriginWhenCrossOrigin()); // Referrer-Policy: strict-origin-when-cross-origin
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
					var result = JsonSerializer.Serialize(new { error = error.Error.Message });
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

	public static void UseCorsOptions(this IApplicationBuilder app, IConfiguration configuration)
	{
		app.UseCors(builder => builder
		.WithOrigins(configuration.GetValue<string>("App:CorsOrigins").Split(";"))
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

	public static void UseSwaggers(this IApplicationBuilder app)
	{
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Yerbowo API v1");
        });
    }
}