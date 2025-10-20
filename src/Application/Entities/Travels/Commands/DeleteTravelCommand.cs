using Application.Common.Interfaces.Repositories;
using Application.Entities.Travels.Exceptions;
using Domain.Travels;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Travels.Commands
{
    public record DeleteTravelCommand : IRequest<Either<TravelException, Travel>>
    {
        public required Guid TravelId { get; init; }
    }

    public class DeleteTravelCommandHandler(ITravelRepository travelRepository)
    : IRequestHandler<DeleteTravelCommand, Either<TravelException, Travel>>
    {
        public async Task<Either<TravelException, Travel>> Handle(
            DeleteTravelCommand request,
            CancellationToken cancellationToken)
        {
            var travelId = new TravelId(request.TravelId);
            var existingTravel = await travelRepository.GetByIdAsync(travelId, cancellationToken);

            return await existingTravel.MatchAsync(
                t => DeleteEntity(t, cancellationToken),
                () => new TravelNotFoundException(travelId));
        }

        private async Task<Either<TravelException, Travel>> DeleteEntity(
            Travel travel,
            CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await travelRepository.DeleteAsync(travel, cancellationToken);
                return deleted;
            }
            catch (Exception ex)
            {
                return new UnhandledTravelException(travel.Id, ex);
            }
        }
    }
}
