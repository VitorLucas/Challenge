using Challenge.Api.Configurations.Extensions;
using Challenge.Application.Abstractions;
using Polly;
using System.Net.Http.Headers;

namespace Challenge.Api.Configurations;

public static class HttpFactoryConfig
{
    public static IServiceCollection AddHttpFactoryConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ISecurityService>(nameof(ISecurityService), client =>
        {
            client.BaseAddress = new Uri(configuration["Security:BaseUrl"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddPolicyHandler(PollyExtensions.WaitAndRetry())
        .AddTransientHttpErrorPolicy(s => s.CircuitBreakerAsync(3, TimeSpan.FromSeconds(15)));

        return services;
    }
}
