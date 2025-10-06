using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Travels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TravelRepository(ApplicationDbContext context) : ITravelRepository, ITravelQueries
{
    public async Task<Travel> AddAsync(Travel entity, CancellationToken cancellationToken)
    {
        await context.Travels.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Travel>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Travels
            .Include(x => x.user)
            .Include(x => x.Members)
            .ToListAsync(cancellationToken);
    }
}
