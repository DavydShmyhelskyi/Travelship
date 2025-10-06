using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                dataSource,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        // Cities
        services.AddScoped<CityRepository>();
        services.AddScoped<ICityRepository>(provider => provider.GetRequiredService<CityRepository>());
        services.AddScoped<ICityQueries>(provider => provider.GetRequiredService<CityRepository>());

        // Countries
        services.AddScoped<CountryRepository>();
        services.AddScoped<ICountryRepository>(provider => provider.GetRequiredService<CountryRepository>());
        services.AddScoped<ICountryQueries>(provider => provider.GetRequiredService<CountryRepository>());

        // Feedbacks
        services.AddScoped<FeedbackRepository>();
        services.AddScoped<IFeedbackRepository>(provider => provider.GetRequiredService<FeedbackRepository>());
        services.AddScoped<IFeedbackQueries>(provider => provider.GetRequiredService<FeedbackRepository>());

        // Followers 
        services.AddScoped<FolowerRepository>();
        services.AddScoped<IFolowerRepository>(provider => provider.GetRequiredService<FolowerRepository>());
        services.AddScoped<IFolowerQueries>(provider => provider.GetRequiredService<FolowerRepository>());

        // Places
        services.AddScoped<PlaceRepository>();
        services.AddScoped<IPlaceRepository>(provider => provider.GetRequiredService<PlaceRepository>());
        services.AddScoped<IPlaceQueries>(provider => provider.GetRequiredService<PlaceRepository>());

        // PlacePhotos
        services.AddScoped<PlacePhotoRepository>();
        services.AddScoped<IPlacePhotoRepository>(provider => provider.GetRequiredService<PlacePhotoRepository>());
        services.AddScoped<IPlacePhotoQueries>(provider => provider.GetRequiredService<PlacePhotoRepository>());

        // Roles
        services.AddScoped<RoleRepository>();
        services.AddScoped<IRoleRepository>(provider => provider.GetRequiredService<RoleRepository>());
        services.AddScoped<IRoleQueries>(provider => provider.GetRequiredService<RoleRepository>());

        // Travels
        services.AddScoped<TravelRepository>();
        services.AddScoped<ITravelRepository>(provider => provider.GetRequiredService<TravelRepository>());
        services.AddScoped<ITravelQueries>(provider => provider.GetRequiredService<TravelRepository>());

        // Users
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository>(provider => provider.GetRequiredService<UserRepository>());
        services.AddScoped<IUserQueries>(provider => provider.GetRequiredService<UserRepository>());
    }
}