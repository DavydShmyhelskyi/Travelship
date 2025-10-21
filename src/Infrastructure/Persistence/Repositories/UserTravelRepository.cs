using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Travels;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserTravelRepository(ApplicationDbContext context) : IUserTravelRepository, IUserTravelQueries
    {


        public async Task<IReadOnlyList<UserTravel>> AddRangeAsync(
            IReadOnlyList<UserTravel> entities,
            CancellationToken cancellationToken)
        {
            await context.UserTravels.AddRangeAsync(entities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entities;
        }

        public async Task<IReadOnlyList<UserTravel>> RemoveRangeAsync(
            IReadOnlyList<UserTravel> entities,
            CancellationToken cancellationToken)
        {
            context.UserTravels.RemoveRange(entities);
            await context.SaveChangesAsync(cancellationToken);
            return entities;
        }

        public async Task<IReadOnlyList<UserTravel>> GetByTravelIdAsync(
            TravelId travelId,
            CancellationToken cancellationToken)
        {
            return await context.UserTravels
                .Include(ut => ut.user)
                .Include(ut => ut.travel)
                .Where(ut => ut.TravelId == travelId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<UserTravel>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.UserTravels
                .Include(ut => ut.user)
                .Include(ut => ut.travel)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
