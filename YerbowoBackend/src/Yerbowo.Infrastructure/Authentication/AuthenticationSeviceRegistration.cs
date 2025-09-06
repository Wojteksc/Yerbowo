namespace Yerbowo.Infrastructure.Authentication;

[ExcludeFromCodeCoverage]
public static class AuthenticationServiceRegistration
{
    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<GoogleAuthOptions>(configuration.GetSection("Authentication:Google"));
        services.Configure<FacebookAuthOptions>(configuration.GetSection("Authentication:Facebook"));

        var jwtOptions = configuration.GetOptions<JwtOptions>("Jwt");
        var googleOptions = configuration.GetOptions<GoogleAuthOptions>("Authentication:Google");
        var facebookOptions = configuration.GetOptions<FacebookAuthOptions>("Authentication:Facebook");

        services
          .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidIssuer = jwtOptions.Issuer,
                  ValidateIssuer = true,

                  ValidAudience = jwtOptions.Audience,
                  ValidateAudience = true,

                  ValidateLifetime = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
              };
          })
          .AddGoogle(opt =>
          {
              opt.ClientId = googleOptions.ClientId;
              opt.ClientSecret = googleOptions.ClientSecret;
          })
          .AddFacebook(opt =>
          {
              opt.ClientId = facebookOptions.ClientId;
              opt.ClientSecret = facebookOptions.ClientSecret;
          });
    }
}