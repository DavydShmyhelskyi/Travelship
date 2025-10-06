using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Cities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CityRepository(ApplicationDbContext context) : ICityRepository, ICityQueries
{
    public async Task<City> AddAsync(City entity, CancellationToken cancellationToken)
    {
        await context.Cities.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Cities.ToListAsync(cancellationToken);
    }
}
