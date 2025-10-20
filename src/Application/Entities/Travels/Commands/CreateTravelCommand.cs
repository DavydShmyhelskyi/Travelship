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
    public required Guid UserId { get; set; } // власник подорожі
}

public class CreateTravelCommandHandler(
    ITravelRepository travelRepository,
    IUserRepository userRepository)
    : IRequestHandler<CreateTravelCommand, Either<TravelException, Travel>>
{
    public async Task<Either<TravelException, Travel>> Handle(CreateTravelCommand request, CancellationToken cancellationToken)
    {
        var existingTravel = await travelRepository.GetByTitleAsync(request.Title, cancellationToken);

        return await existingTravel.MatchAsync(
            t => new TravelAlreadyExistsException(t.Id),
            () => ValidateMembers(request, cancellationToken)
                .BindAsync(_ => CreateEntity(request, cancellationToken))
        );
    }

    private async Task<Either<TravelException, Unit>> ValidateMembers(
        CreateTravelCommand request,
        CancellationToken cancellationToken)
    {
        var allMemberIds = request.Members.Append(request.UserId).Distinct().ToList();
        var existingMembers = await userRepository.GetByIdsAsync(
            allMemberIds.Select(x => new UserId(x)).ToList(),
            cancellationToken
        );

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
                .Append(request.UserId) // власник теж учасник
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
        catch (Exception ex)
        {
            return new UnhandledTravelException(TravelId.Empty(), ex);
        }
    }
}
