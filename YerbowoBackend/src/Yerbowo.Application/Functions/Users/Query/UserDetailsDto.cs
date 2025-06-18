namespace Yerbowo.Application.Functions.Users.Query;

public record UserDetailsDto
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string CompanyName { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }
}