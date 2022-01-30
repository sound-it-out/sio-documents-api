using SIO.Documents.API.V1.Endpoints;

namespace SIO.Documents.API.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
            => builder.MapV1Endpoints();
    }
}
