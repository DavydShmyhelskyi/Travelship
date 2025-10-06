using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository, IUserQueries
{
    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Users
            .Include(x => x.Role)
            .Include(x => x.City)
            .Include(x => x.Travels)
            .ToListAsync(cancellationToken);
    }
}
