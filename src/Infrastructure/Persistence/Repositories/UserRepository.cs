using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository, IUserQueries
    {
        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        public async Task<User> AddAsync(User entity, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<User> UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            context.Users.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<User> DeleteAsync(User entity, CancellationToken cancellationToken)
        {
            context.Users.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Option<User>> GetByNickNameAsync(string nickname, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.NickName == nickname, cancellationToken);
            return user == null ? Option<User>.None : Option<User>.Some(user);
        }

        public async Task<Option<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => EF.Functions.ILike(u.Email, email),
                    cancellationToken);

            return user == null ? Option<User>.None : Option<User>.Some(user);
        }


        public async Task<Option<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .Include(u => u.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            return user == null ? Option<User>.None : Option<User>.Some(user);
        }

        public async Task<IReadOnlyList<User>> GetByIdsAsync(IReadOnlyList<UserId> userIds, CancellationToken cancellationToken)
        {
            var idValues = userIds.Select(x => x.Value).ToList();

            return await context.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken)        // ✅ спочатку тягнемо все в пам’ять
                .ContinueWith(t =>
                    (IReadOnlyList<User>)t.Result
                        .Where(u => idValues.Contains(u.Id.Value))
                        .ToList(),
                    cancellationToken);
        }

    }
}
