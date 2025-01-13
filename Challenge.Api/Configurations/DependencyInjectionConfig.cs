using Challenge.Application;
using Challenge.Application.Abstractions;

namespace Challenge.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Application(configuration);
        services.Repositories();
        services.Services();

        return services;
    }

    private static void Application(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpFactoryConfiguration(configuration);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    private static void Services(this IServiceCollection services)
    {
        services.AddScoped<ISecurityService, SecurityService>();
    }

    private static void Repositories(this IServiceCollection services)
    {

    }
}
