using MediatR;

namespace Yerbowo.Domain.RequestProcessing.Command;

public interface ICommand<out TResult> : IRequest<TResult> { }

public interface ICommand : IRequest { }