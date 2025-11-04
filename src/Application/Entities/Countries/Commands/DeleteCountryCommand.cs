using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Countries.Exceptions;
using Domain.Countries;
using LanguageExt;
using LanguageExt.ClassInstances;
using MediatR;

namespace Application.Entities.Countries.Commands
{
    public record DeleteCountryCommand : IRequest<Either<CountryException, Country>>
    {
        public required Guid CountryId { get; init; }
    }
    public class DeleteCountryCommandHandler (ICountryRepository countryRepository, ICountryQueries countryQueries) : IRequestHandler<DeleteCountryCommand, Either<CountryException, Country>>
    {
        public async Task<Either<CountryException, Country>> Handle(
        DeleteCountryCommand request,
        CancellationToken cancellationToken)
        {
            var countryId = new CountryId(request.CountryId);
            var country = await countryQueries.GetByIdAsync(countryId, cancellationToken);
            return await country.MatchAsync(
                p => DeleteEntity(p, cancellationToken),
                () => new CountryNotFoundException(countryId));
        }

        private async Task<Either<CountryException, Country>> DeleteEntity(
            Country country,
            CancellationToken cancellationToken)
        {
            try
            {
                return await countryRepository.DeleteAsync(country, cancellationToken);
            }
            catch (Exception exception)
            {
                return new UnhandledCountryException(country.Id, exception);
            }
        }
    }
}
