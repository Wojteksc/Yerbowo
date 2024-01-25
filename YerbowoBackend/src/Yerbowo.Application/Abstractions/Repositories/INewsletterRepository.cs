namespace Yerbowo.Application.Abstractions.Repositories;

public interface INewsletterRepository : IDbEntityRepository<Newsletter>
{
    Task<Newsletter> GetAsync(string email);
}