namespace Yerbowo.Infrastructure.Services.RequestProcessing;

[ExcludeFromCodeCoverage]
public class RequestDispatcher(IMediator mediator) : IRequestDispatcher
{
    public async Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task ExecuteCommand(ICommand command, CancellationToken cancellationToken = default)
    {
        await mediator.Send(command, cancellationToken);
    }

    public async Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(query, cancellationToken);
    }

    public async Task Publish(Domain.RequestProcessing.Notification.INotification notification, CancellationToken cancellationToken = default)
    {
        await mediator.Publish(notification, cancellationToken);
    }
}