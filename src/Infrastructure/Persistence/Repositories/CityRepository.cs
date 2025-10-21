using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Cities;
using Domain.Countries;
using LanguageExt;
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

    public async Task<City> UpdateAsync(City entity, CancellationToken cancellationToken)
    {
        context.Cities.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<City> DeleteAsync(City entity, CancellationToken cancellationToken)
    {
        context.Cities.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<City>> GetByTitleAsync(
        string title,
        CountryId countryId,
        CancellationToken cancellationToken)
    {
        var city = await context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Title == title && c.CountryId == countryId,
                cancellationToken);

        return city ?? Option<City>.None;
    }

    public async Task<Option<City>> GetByIdAsync(CityId id, CancellationToken cancellationToken)
    {
        var city = await context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return city ?? Option<City>.None;
    }

    public async Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Cities
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
