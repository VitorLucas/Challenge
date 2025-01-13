using Challenge.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text.Json.Serialization;

namespace Challenge.Api.Configurations;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddCors(options =>
        {
            options.AddPolicy("Development",
                builder =>
                    builder
                        .WithOrigins()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithHeaders(HeaderNames.ContentType, "Access-Control-Allow-Origin"));

            options.AddPolicy("Staging",
                builder =>
                    builder
                        .WithOrigins("https://localhost:5003", "http://localhost:5004")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithHeaders(HeaderNames.ContentType, "Access-Control-Allow-Origin"));

            options.AddPolicy("Production",
                builder =>
                    builder
                        .WithOrigins("https://localhost:5005",
                                     "http://localhost:5006")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithHeaders(HeaderNames.ContentType, "Access-Control-Allow-Origin"));
        });

        //var credentialsSection = configuration.GetSection("Credentials");
        //services.Configure<Credentials>(credentialsSection);

        //services.AddDbContext<BoondManagerDbContext>(option =>
        //{
        //    option.UseNpgsql(configuration["SyncConnectionString"])
        //        .EnableSensitiveDataLogging();
        //    option.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
        //       category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)));
        //});

        return services;
    }

    public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsStaging())
        {
            app.UseCors("Development");
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseCors("Production");
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}

