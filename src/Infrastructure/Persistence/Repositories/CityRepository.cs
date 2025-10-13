using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Cities;
using Domain.Countries;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CityRepository : ICityRepository, ICityQueries
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        _context = context;
    }

    public async Task<City> AddAsync(City entity, CancellationToken cancellationToken)
    {
        await _context.Cities.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<City> UpdateAsync(City entity, CancellationToken cancellationToken)
    {
        _context.Cities.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<City> DeleteAsync(City entity, CancellationToken cancellationToken)
    {
        _context.Cities.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<City>> GetByTitleAsync(
        string title,
        CountryId countryId,
        CancellationToken cancellationToken)
    {
        var city = await _context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Title == title && c.CountryId == countryId,
                cancellationToken);

        return city ?? Option<City>.None;
    }

    public async Task<Option<City>> GetByIdAsync(CityId id, CancellationToken cancellationToken)
    {
        var city = await _context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return city ?? Option<City>.None;
    }

    public async Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Cities
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
