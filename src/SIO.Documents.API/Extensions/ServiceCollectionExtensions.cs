using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SIO.Documents.Api.OpenApi;
using SIO.Domain.Extensions;
using SIO.Infrastructure.Azure.Storage.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.SqlServer.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Serialization.Json.Extensions;
using SIO.Infrastructure.Serialization.MessagePack.Extensions;

namespace SIO.Documents.API.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = configuration.GetValue<string>("Identity:Authority");
                        options.ApiName = configuration.GetValue<string>("Identity:ApiResource");
                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromMinutes(10);

                        if (env.IsDevelopment())
                        {
                            options.RequireHttpsMetadata = false;
                            IdentityModelEventSource.ShowPII = true;
                        }

                        options.TokenRetriever = (request) =>
                        {
                            var token = TokenRetrieval.FromAuthorizationHeader()(request);

                            if (string.IsNullOrEmpty(token))
                                token = TokenRetrieval.FromQueryString()(request);

                            return token;
                        };
                    });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = services.AddSIOInfrastructure();

            builder.AddEntityFrameworkCoreSqlServer(options => {
                options.AddStore<SIOStoreDbContext>(configuration.GetConnectionString("Store"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                options.AddProjections(configuration.GetConnectionString("Projection"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
            })
            .AddEntityFrameworkCoreStoreProjector(options => options.WithDomainProjections())
            .AddAzureStorage(o => o.ConnectionString = configuration.GetConnectionString("AzureStorage"))
            .AddCommands()
            .AddEvents(options =>
            {
                options.Register(new IntegrationEvents.AllEvents().ToArray());
            })
            .AddQueries()
            .AddJsonSerializers();

            return services;
        }
    }
}
