using MediatR;

namespace Yerbowo.Domain.RequestProcessing.Query;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }