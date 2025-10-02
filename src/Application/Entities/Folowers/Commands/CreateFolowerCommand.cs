using Application.Common.Interfaces.Repositories;
using Domain.Folowers;
using MediatR;

namespace Application.Entities.Folowers.Commands;

public class CreateFollowerCommand : IRequest<Folower>
{
    public required Guid FollowerId { get; set; }
    public required Guid FollowedId { get; set; }
}
public class CreateFollowerCommandHandler(IFolowerRepository folowerRepository)
    : IRequestHandler<CreateFollowerCommand, Folower>
{
    public async Task<Folower> Handle(CreateFollowerCommand request, CancellationToken cancellationToken)
    {
        var folower = Folower.New(request.FollowerId, request.FollowedId);
        return await folowerRepository.AddAsync(folower, cancellationToken);
    }
}
