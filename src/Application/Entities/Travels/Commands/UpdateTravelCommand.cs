using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Travels.Exceptions;
using Domain.Places;
using Domain.Travels;
using Domain.Users;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Travels.Commands;

public record UpdateTravelCommand : IRequest<Either<TravelException, Travel>>
{
    public required Guid TravelId { get; init; }
    public required string Title { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required string Description { get; init; }
    public byte[]? Image { get; init; }
    public bool IsDone { get; init; }

    public required IReadOnlyList<Guid> Places { get; init; }
    public required IReadOnlyList<Guid> Members { get; init; }
}

public class UpdateTravelCommandHandler(
    IApplicationDbContext applicationDbContext,
    ITravelRepository travelRepository,
    ITravelPlaceRepository travelPlaceRepository,
    IUserTravelRepository userTravelRepository,
    IPlaceRepository placeRepository,
    IUserRepository userRepository)
    : IRequestHandler<UpdateTravelCommand, Either<TravelException, Travel>>
{
    public async Task<Either<TravelException, Travel>> Handle(UpdateTravelCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await applicationDbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await HandleAsync(request, cancellationToken);

            if (result.IsLeft)
                transaction.Rollback();
            else
                transaction.Commit();

            return result;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new UnhandledTravelException(TravelId.Empty(), ex);
        }
    }

    private async Task<Either<TravelException, Travel>> HandleAsync(UpdateTravelCommand request, CancellationToken cancellationToken)
    {
        var travelId = new TravelId(request.TravelId);
        var travel = await travelRepository.GetByIdAsync(travelId, cancellationToken);

        return await travel.MatchAsync(
            async t =>
                await CheckPlaces(request.Places, cancellationToken)
                    .BindAsync(_ => CheckMembers(request.Members, cancellationToken)
                        .BindAsync(_ => UpdateEntity(request, t, cancellationToken)
                            .BindAsync(updated =>
                                UpdateRelations(updated, request.Places, request.Members, cancellationToken)))),
            () => new TravelNotFoundException(travelId));
    }

    private async Task<Either<TravelException, Unit>> CheckPlaces(
        IReadOnlyList<Guid> places,
        CancellationToken cancellationToken)
    {
        var existingPlaces = await placeRepository.GetByIdsAsync(
            places.Select(x => new PlaceId(x)).ToList(),
            cancellationToken);

        var missing = places.Where(x => existingPlaces.All(p => p.Id.Value != x)).ToList();

        return missing.Any()
            ? new PlacesNotFoundException(TravelId.Empty(), missing)
            : Unit.Default;
    }

    private async Task<Either<TravelException, Unit>> CheckMembers(
        IReadOnlyList<Guid> members,
        CancellationToken cancellationToken)
    {
        var existingUsers = await userRepository.GetByIdsAsync(
            members.Select(x => new UserId(x)).ToList(),
            cancellationToken);

        var missing = members.Where(x => existingUsers.All(u => u.Id.Value != x)).ToList();

        return missing.Any()
            ? new MembersNotFoundException(TravelId.Empty(), missing)
            : Unit.Default;
    }

    private async Task<Either<TravelException, Travel>> UpdateEntity(
        UpdateTravelCommand request,
        Travel travel,
        CancellationToken cancellationToken)
    {
        try
        {
            travel.Update(
                request.Title,
                request.StartDate,
                request.EndDate,
                request.Description,
                request.Image,
                request.IsDone,
                travel.Members ?? new List<UserTravel>(),
                travel.Places ?? new List<TravelPlace>());

            return await travelRepository.UpdateAsync(travel, cancellationToken);
        }
        catch (Exception ex)
        {
            return new UnhandledTravelException(travel.Id, ex);
        }
    }

    private async Task<Either<TravelException, Travel>> UpdateRelations(
        Travel travel,
        IReadOnlyList<Guid> newPlaces,
        IReadOnlyList<Guid> newMembers,
        CancellationToken cancellationToken)
    {
        var placesResult = await UpdatePlacesAssignments(travel, newPlaces, cancellationToken);
        if (placesResult.IsLeft)
            return placesResult;

        var membersResult = await UpdateMembersAssignments(travel, newMembers, cancellationToken);
        return membersResult;
    }

    private async Task<Either<TravelException, Travel>> UpdatePlacesAssignments(
        Travel travel,
        IReadOnlyList<Guid> newPlaces,
        CancellationToken cancellationToken)
    {
        try
        {
            var existing = await travelPlaceRepository.GetByTravelIdAsync(travel.Id, cancellationToken);
            var newIds = newPlaces.Select(p => new PlaceId(p)).ToList();

            var toAdd = newIds.Where(pid => existing.All(ep => ep.PlaceId != pid)).ToList();
            var toRemove = existing.Where(ep => newIds.All(pid => pid != ep.PlaceId)).ToList();

            if (toAdd.Any())
                await travelPlaceRepository.AddRangeAsync(
                    toAdd.Select(pid => TravelPlace.New(travel.Id, pid)).ToList(),
                    cancellationToken);

            if (toRemove.Any())
                await travelPlaceRepository.RemoveRangeAsync(toRemove, cancellationToken);

            return travel;
        }
        catch (Exception ex)
        {
            return new UnhandledTravelException(travel.Id, ex);
        }
    }

    private async Task<Either<TravelException, Travel>> UpdateMembersAssignments(
        Travel travel,
        IReadOnlyList<Guid> newMembers,
        CancellationToken cancellationToken)
    {
        try
        {
            var existing = await userTravelRepository.GetByTravelIdAsync(travel.Id, cancellationToken);
            var newIds = newMembers.Select(u => new UserId(u)).ToList();

            var toAdd = newIds.Where(uid => existing.All(em => em.UserId != uid)).ToList();
            var toRemove = existing.Where(em => newIds.All(uid => uid != em.UserId)).ToList();

            if (toAdd.Any())
                await userTravelRepository.AddRangeAsync(
                    toAdd.Select(uid => UserTravel.New(uid, travel.Id)).ToList(),
                    cancellationToken);

            if (toRemove.Any())
                await userTravelRepository.RemoveRangeAsync(toRemove, cancellationToken);

            return travel;
        }
        catch (Exception ex)
        {
            return new UnhandledTravelException(travel.Id, ex);
        }
    }
}
