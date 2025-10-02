using Application.Common.Interfaces.Repositories;
using Domain.Countries;
using MediatR;

namespace Application.Entities.Countries.Commands
{
    public class CreateCountryCommand : IRequest<Country>
    {
        public required string Title { get; set; }
    }
    public class CreateCountryCommandHandler(ICountryRepository countryRepository) : IRequestHandler<CreateCountryCommand, Country>
    {
        public async Task<Country> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await countryRepository.AddAsync(Country.New(request.Title), cancellationToken);
            return country;
        }
    }
}
