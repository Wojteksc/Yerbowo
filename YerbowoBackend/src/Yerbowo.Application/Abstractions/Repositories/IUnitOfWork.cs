namespace Yerbowo.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}