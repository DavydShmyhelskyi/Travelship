using System.Data;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    /// <summary>
    /// Added for education purposes
    /// </summary>
    Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}