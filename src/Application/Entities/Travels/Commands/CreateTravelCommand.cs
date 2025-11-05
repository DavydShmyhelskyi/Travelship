using Application.Common.Interfaces.Repositories;
using Application.Entities.Travels.Exceptions;
using Domain.Places;
using Domain.Travels;
using Domain.Users;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Travels.Commands;

public class CreateTravelCommand : IRequest<Either<TravelException, Travel>>
{
    public required string Title { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required string Description { get; init; }
    public byte[]? Image { get; init; }
    public bool IsDone { get; init; }
    public required IReadOnlyList<Guid> Places { get; init; }
    public required IReadOnlyList<Guid> Members { get; init; }
    public required Guid UserId { get; set; }
}

public class CreateTravelCommandHandler(
    ITravelRepository travelRepository,
    IUserRepository userRepository,
    IPlaceRepository placeRepository)
    : IRequestHandler<CreateTravelCommand, Either<TravelException, Travel>>
{
    public async Task<Either<TravelException, Travel>> Handle(CreateTravelCommand request, CancellationToken cancellationToken)
    {
        var existingTravel = await travelRepository.GetByTitleAsync(request.Title, cancellationToken);

        return await existingTravel.MatchAsync(
            t => new TravelAlreadyExistsException(t.Id),
            async () =>
            {
                var ownerCheck = await ValidateOwnerExists(request, cancellationToken);
                return await ownerCheck.MatchAsync<Either<TravelException, Travel>>(
                    Left: err => err,
                    RightAsync: async _ =>
                    {
                        var placesResult = await ValidatePlaces(request, cancellationToken);
                        return await placesResult.MatchAsync<Either<TravelException, Travel>>(
                            Left: err => err,
                            RightAsync: async _ =>
                            {
                                var membersResult = await ValidateMembers(request, cancellationToken);
                                return await membersResult.MatchAsync<Either<TravelException, Travel>>(
                                    Left: err => err,
                                    RightAsync: async _ => await CreateEntity(request, cancellationToken)
                                );
                            });
                    });
            });
    }

    private async Task<Either<TravelException, Unit>> ValidateOwnerExists(
        CreateTravelCommand request,
        CancellationToken cancellationToken)
    {
        var owner = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);

        return await owner.MatchAsync(
            Some: _ => Task.FromResult<Either<TravelException, Unit>>(Unit.Default),
            None: () => Task.FromResult<Either<TravelException, Unit>>(
                new MembersNotFoundException(TravelId.New(), new List<Guid> { request.UserId }))
        );
    }


    private async Task<Either<TravelException, Unit>> ValidatePlaces(
        CreateTravelCommand request,
        CancellationToken cancellationToken)
    {
        var existingPlaces = await placeRepository.GetByIdsAsync(
            request.Places.Select(x => new PlaceId(x)).ToList(),
            cancellationToken);

        var missing = request.Places
            .Where(x => existingPlaces.All(p => p.Id.Value != x))
            .ToList();

        return missing.Any()
            ? new PlacesNotFoundException(TravelId.New(), missing)
            : Unit.Default;
    }

    private async Task<Either<TravelException, Unit>> ValidateMembers(
        CreateTravelCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Members == null || !request.Members.Any())
            return new MembersNotFoundException(TravelId.Empty(), new List<Guid>());

        var allMemberIds = request.Members.Append(request.UserId).Distinct().ToList();
        var existingMembers = await userRepository.GetByIdsAsync(
            allMemberIds.Select(x => new UserId(x)).ToList(),
            cancellationToken);

        var missing = allMemberIds
            .Where(x => existingMembers.All(e => e.Id.Value != x))
            .ToList();

        return missing.Any()
            ? new MembersNotFoundException(TravelId.Empty(), missing)
            : Unit.Default;
    }


    private async Task<Either<TravelException, Travel>> CreateEntity(
        CreateTravelCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var travelId = TravelId.New();

            var places = request.Places
                .Select(x => TravelPlace.New(travelId, new PlaceId(x)))
                .ToList();

            var members = request.Members
                .Append(request.UserId)
                .Distinct()
                .Select(x => UserTravel.New(new UserId(x), travelId))
                .ToList();

            var travel = Travel.New(
                travelId,
                request.Title,
                request.StartDate,
                request.EndDate,
                request.Description,
                request.Image,
                new UserId(request.UserId),
                members,
                places
            );

            var created = await travelRepository.AddAsync(travel, cancellationToken);
            return created;
        }
        catch (TravelException tex)
        {
            return tex;
        }
        catch (Exception ex)
        {
            return new UnhandledTravelException(TravelId.New(), ex);
        }
    }
}
