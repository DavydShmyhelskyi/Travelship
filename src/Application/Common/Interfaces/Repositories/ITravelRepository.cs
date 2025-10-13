using Domain.Travels;
using Domain.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITravelRepository
    {
        Task<Travel> AddAsync(Travel entity, CancellationToken cancellationToken);
        Task<Travel> UpdateAsync(Travel entity, CancellationToken cancellationToken);
        Task<Travel> DeleteAsync(Travel entity, CancellationToken cancellationToken);
        Task<Option<Travel>> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Option<Travel>> GetByIdAsync(TravelId id, CancellationToken cancellationToken);
        Task<Option<Travel>> GetByUserAsync(UserId userId, CancellationToken cancellationToken);

    }
}
