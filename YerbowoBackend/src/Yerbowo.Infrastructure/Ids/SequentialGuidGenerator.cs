namespace Yerbowo.Infrastructure.Ids;

internal class SequentialGuidGenerator : IIdGenerator
{
    public Guid Generate() => Guid.CreateVersion7();
}