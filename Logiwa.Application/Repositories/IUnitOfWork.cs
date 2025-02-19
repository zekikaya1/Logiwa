using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Logiwa.Application.Repositories;

public interface IUnitOfWork
{
    IDbContextTransaction BeginTransaction();
    IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    void RollbackTransaction();
    void CommitTransaction();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}