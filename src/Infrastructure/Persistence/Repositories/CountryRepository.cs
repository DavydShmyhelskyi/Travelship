using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Countries;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CountryRepository : ICountryRepository, ICountryQueries
{
    private readonly ApplicationDbContext _context;

    public CountryRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        _context = context;
    }
    public async Task<Country> AddAsync(Country entity, CancellationToken cancellationToken)
    {
        await _context.Countries.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Country>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Countries.AsNoTracking().ToListAsync(cancellationToken); 
    }


    public async Task<Country> UpdateAsync(Country entity, CancellationToken cancellationToken)
    {
        _context.Countries.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    public async Task<Country> DeleteAsync(Country entity, CancellationToken cancellationToken)
    {
        _context.Countries.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    public async Task<Option<Country>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var country = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Title == title, cancellationToken);
        return country ?? Option<Country>.None;
    }
    public async Task<Option<Country>> GetByIdAsync(CountryId id, CancellationToken cancellationToken)
    {
        var country = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return country ?? Option<Country>.None;
    }
}
