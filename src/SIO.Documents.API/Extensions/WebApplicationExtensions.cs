using SIO.Domain.Extensions;

namespace SIO.Documents.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplicationBuilder ConfigureDocumentsApi(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuthentication(builder.Configuration, builder.Environment)
                .AddInfrastructure(builder.Configuration)
                .AddDomain()
                .AddCors();

            return builder;
        }
    }
}
