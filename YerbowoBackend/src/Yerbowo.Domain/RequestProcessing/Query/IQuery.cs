using MediatR;

namespace Yerbowo.Domain.RequestProcessing.Query;

public interface IQuery<out TResult> : IRequest<TResult> { }