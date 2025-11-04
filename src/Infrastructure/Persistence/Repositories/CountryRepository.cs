using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Countries;
using LanguageExt;
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
        return await context.Countries.AsNoTracking().ToListAsync(cancellationToken); 
    }


    public async Task<Country> UpdateAsync(Country entity, CancellationToken cancellationToken)
    {
        context.Countries.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    public async Task<Country> DeleteAsync(Country entity, CancellationToken cancellationToken)
    {
        context.Countries.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    public async Task<Option<Country>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var country = await context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => EF.Functions.ILike(c.Title, title), cancellationToken);

        return country ?? Option<Country>.None;
    }

    public async Task<Option<Country>> GetByIdAsync(CountryId id, CancellationToken cancellationToken)
    {
        var country = await context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return country ?? Option<Country>.None;
    }
}
