using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Entities.Countries.Exceptions;
using Domain.Countries;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Entities.Countries.Commands;

public record UpdateCountryCommand : IRequest<Either<CountryException, Country>>
{
    public required Guid CountryId { get; init; }
    public required string Title { get; init; }
}

public class UpdateCountryCommandHandler(
    ICountryRepository countryRepository,
    ICountryQueries countryQueries
) : IRequestHandler<UpdateCountryCommand, Either<CountryException, Country>>
{
    public async Task<Either<CountryException, Country>> Handle(
        UpdateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var countryId = new CountryId(request.CountryId);

        var country = await countryQueries.GetByIdAsync(countryId, cancellationToken);

        return await country.MatchAsync(
            c => CheckDuplicates(c.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, c, cancellationToken)),
            () => new CountryNotFoundException(countryId));
    }

    private async Task<Either<CountryException, Country>> UpdateEntity(
        UpdateCountryCommand request,
        Country country,
        CancellationToken cancellationToken)
    {
        try
        {
            country.ChangeTitle(request.Title);
            return await countryRepository.UpdateAsync(country, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UnhandledCountryException(country.Id, exception);
        }
    }

    private async Task<Either<CountryException, Unit>> CheckDuplicates(
        CountryId currentCountryId,
        string title,
        CancellationToken cancellationToken)
    {
        var existing = await countryQueries.GetByTitleAsync(title, cancellationToken);

        return existing.Match<Either<CountryException, Unit>>(
            c => c.Id.Equals(currentCountryId) ? Unit.Default : new CountryAlreadyExistException(c.Id),
            () => Unit.Default);
    }
}