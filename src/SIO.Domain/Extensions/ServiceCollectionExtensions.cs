using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain.Documents.CommandHandlers;
using SIO.Domain.Documents.Commands;
using SIO.Domain.Documents.Queries;
using SIO.Domain.Documents.QueryHandlers;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddDocuments()
                .AddMemoryCache();
        }

        public static IServiceCollection AddDocuments(this IServiceCollection services)
        {
            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            //Commands
            services.AddScoped<ICommandHandler<UploadDocumentCommand>, UploadDocumentCommandHandler>();

            //Queries
            services.AddScoped<IQueryHandler<GetDocumentsForUserQuery, GetDocumentsForUserQueryResult>, GetDocumentsForUserQueryHandler>();
            services.AddScoped<IQueryHandler<GetDocumentStreamQuery, GetDocumentStreamQueryResult>, GetDocumentStreamQueryHandler>();

            return services;
        }
    }
}
