namespace Yerbowo.Infrastructure.Data.Newsletters;

public class NewsletterRepository : DbEntityRepository<Newsletter>, INewsletterRepository
{
    public NewsletterRepository(YerbowoContext db) : base(db)
    {
    }

    public async Task<Newsletter> GetAsync(string email)
    {
        return await _entitiesNotRemoved.SingleOrDefaultAsync(x => x.Email == email);
    }
}