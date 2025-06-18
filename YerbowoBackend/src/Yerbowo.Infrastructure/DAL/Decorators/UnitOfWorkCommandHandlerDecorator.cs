namespace Yerbowo.Infrastructure.DAL.Decorators;

internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand>(
    IRequestHandler<TCommand> commandHandler,
    IUnitOfWork unitOfWork) : IRequestHandler<TCommand>
        where TCommand : class, ICommand
{
    public async Task Handle(TCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteAsync(() => commandHandler.Handle(request, cancellationToken));
    }
}

internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand, TResult>(
    IRequestHandler<TCommand, TResult> commandHandler,
    IUnitOfWork unitOfWork) : IRequestHandler<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        TResult result = default!;

        await unitOfWork.ExecuteAsync(async () =>
        {
            result = await commandHandler.Handle(request, cancellationToken);
        });

        return result;
    }
}