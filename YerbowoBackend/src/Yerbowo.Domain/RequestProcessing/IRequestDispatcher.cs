namespace Yerbowo.Domain.RequestProcessing;

public interface IRequestDispatcher
{
    Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command, CancellationToken cancellation = default);

    Task ExecuteCommand(ICommand command, CancellationToken cancellation = default);

    Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query, CancellationToken cancellation = default);

    Task Publish(INotification notification, CancellationToken cancellation = default);
}