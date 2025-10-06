using FluentValidation;

namespace Api.Modules;

public static class SetupModule
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddCors();
        services.AddRequestValidators();
    }

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));
    }

    private static void AddRequestValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
    }
}