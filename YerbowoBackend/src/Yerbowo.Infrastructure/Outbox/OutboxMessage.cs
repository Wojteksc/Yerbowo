namespace Yerbowo.Infrastructure.Outbox;

public class OutboxMessage : BaseEntity
{
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime? ProccessedAt { get; set; }
    public string Error { get; set; }
}