using Application.Common.Interfaces.Repositories;
using Domain.Travels;
using MediatR;

namespace Application.Entities.Travels.Commands;

public class CreateTravelCommand : IRequest<Travel>
{
    public required string Title { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public byte[]? Image { get; set; }
    public bool IsDone { get; set; }

    public required Guid UserId { get; set; } // власник подорожі
}
public class CreateTravelCommandHandler(ITravelRepository travelRepository)
    : IRequestHandler<CreateTravelCommand, Travel>
{
    public async Task<Travel> Handle(CreateTravelCommand request, CancellationToken cancellationToken)
    {
        var travel = Travel.New(
            request.Title,
            request.StartDate,
            request.EndDate,
            request.Description ?? string.Empty,
            request.Image,
            request.IsDone,
            request.UserId
        );

        return await travelRepository.AddAsync(travel, cancellationToken);
    }
}