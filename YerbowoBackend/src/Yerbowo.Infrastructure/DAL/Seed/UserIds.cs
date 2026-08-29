namespace Yerbowo.Infrastructure.DAL.Seed;

public static class UserIds
{
    public static readonly Guid Admin =
        Guid.Parse("10000000-0000-0000-0000-000000000001");

    public static readonly Guid SimpleUser =
        Guid.Parse("10000000-0000-0000-0000-000000000002");

    public static readonly Guid IntegrationTestUser =
        Guid.Parse("10000000-0000-0000-0000-000000000003");
}