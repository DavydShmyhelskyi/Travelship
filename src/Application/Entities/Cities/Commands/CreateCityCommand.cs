using Application.Common.Interfaces.Repositories;
using Domain.Cities;
using Domain.Countries;
using MediatR;

namespace Application.Entities.Cities.Commands;

public class CreateCityCommand : IRequest<City>
{
    public required string Title { get; set; }
}
public class CreateCityCommandHandler(ICityRepository cityRepository)
    : IRequestHandler<CreateCityCommand, City>
{
    public async Task<City> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var city = City.New(request.Title, CountryId.New());
        return await cityRepository.AddAsync(city, cancellationToken);
    }
}