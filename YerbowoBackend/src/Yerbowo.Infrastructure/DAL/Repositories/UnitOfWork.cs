
namespace Yerbowo.Infrastructure.DAL.Repositories;

internal class UnitOfWork(YerbowoContext context) : IUnitOfWork
{
    public async Task ExecuteAsync(Func<Task> action)
    {
        if (IsInExistingTransaction())
        {
            await ExecuteWithinExistingTransaction(action);
        }
        else
        {
            await ExecuteWithinNewTransaction(action);
        }
    }

    private bool IsInExistingTransaction()
        => context.Database.CurrentTransaction != null;

    /// <summary>
    /// Executes a given action within the context of an already active database transaction.
    ///
    /// This method is used when a higher-level operation has already started a transaction,
    /// so it avoids creating or committing a new one.
    ///
    /// This prevents errors like "The connection is already in a transaction and cannot participate in another transaction."
    ///
    /// See <see cref="ExecuteWithinNewTransaction"/> for the complementary method
    /// that handles starting and committing a new transaction when none exists.
    ///
    /// This design ensures transaction boundaries are respected and managed by the original transaction owner.
    /// </summary>
    private async Task ExecuteWithinExistingTransaction(Func<Task> action)
    {
        await action();
        await context.SaveChangesAsync();
    }

    private async Task ExecuteWithinNewTransaction(Func<Task> action)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await action();
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}