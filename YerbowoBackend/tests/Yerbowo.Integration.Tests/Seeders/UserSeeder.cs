namespace Yerbowo.Integration.Tests.Seeders;

public sealed class UserSeeder(IServiceScopeFactory scopeFactory)
{
    public const string DefaultEmail = "yerbowoTestUser@IntegrationTestYerbowo.com";
    public const string DefaultPassword = "Haslo123.";

    public async Task<Guid> SeedAsync(
        string email = DefaultEmail,
        string password = DefaultPassword,
        string firstName = "FirstName",
        string lastName = "LastName",
        string role = "admin")
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<YerbowoContext>();
        var passwordManager = scope.ServiceProvider.GetRequiredService<IPasswordManager>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var existingUser = await context.Users
            .SingleOrDefaultAsync(x => x.Email == email);

        if (existingUser is not null)
            return existingUser.Id;

        var user = new User(
            id: Guid.NewGuid(),
            firstName: firstName,
            lastName: lastName,
            email: email,
            role: role);

        user.SetVerificationDate(DateTime.UtcNow);
        user.SetVerificationToken(Guid.NewGuid().ToString("N"));
        user.SetPassword(passwordManager.Secure(password));

        await unitOfWork.ExecuteAsync(() =>
        {
            context.Users.Add(user);
            return Task.CompletedTask;
        });

        return user.Id;
    }
}