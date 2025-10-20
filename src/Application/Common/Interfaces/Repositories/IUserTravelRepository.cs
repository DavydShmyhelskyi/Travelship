using Domain.Travels;
using Domain.Users;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUserTravelRepository
    {
        Task<IReadOnlyList<UserTravel>> AddRangeAsync(
        IReadOnlyList<UserTravel> entities,
        CancellationToken cancellationToken);

        Task<IReadOnlyList<UserTravel>> RemoveRangeAsync(
            IReadOnlyList<UserTravel> entities,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<UserTravel>> GetByTravelIdAsync(
            TravelId travelId,
            CancellationToken cancellationToken);
    }
}
