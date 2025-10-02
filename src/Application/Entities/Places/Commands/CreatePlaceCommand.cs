using Domain.Places;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Places.Commands
{
    public class CreatePlaceCommand : IRequest<Place>
    {
        public required string Title { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
    public class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand, Place>
    {
        public Task<Place> Handle(CreatePlaceCommand request, CancellationToken cancellationToken)
        {
            var place = Place.New(request.Title, request.Latitude, request.Longitude);
            return Task.FromResult(place);
        }
    }
}
