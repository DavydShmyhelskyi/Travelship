using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Countries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CountryRepository(ApplicationDbContext context) : ICountryRepository, ICountryQueries
{
    public async Task<Country> AddAsync(Country entity, CancellationToken cancellationToken)
    {
        await context.Countries.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Countries.ToListAsync(cancellationToken);
    }
}
