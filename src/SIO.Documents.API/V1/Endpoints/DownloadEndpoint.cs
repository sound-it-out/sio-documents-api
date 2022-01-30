using Microsoft.AspNetCore.Mvc;
using SIO.Documents.API.Extensions;
using SIO.Domain.Documents.Queries;
using SIO.Infrastructure;
using SIO.Infrastructure.Queries;
using System.Security.Claims;

namespace SIO.Documents.API.V1.Endpoints
{
    public static class DownloadEndpoint
    {
        public static IEndpointRouteBuilder MapDownloadEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("v1/{id}/download", DownloadAsync)
                .RequireAuthorization();

            return builder;
        }

        private static async Task<IResult> DownloadAsync([FromServices] IQueryDispatcher queryDispatcher,
            [FromRoute] string id,
            ClaimsPrincipal user,
            CancellationToken cancellationToken)
        {
            var documentStreamResult = await queryDispatcher.DispatchAsync(new GetDocumentStreamQuery(CorrelationId.New(), user.Actor(), id), cancellationToken);
            return Results.File(documentStreamResult.Stream, documentStreamResult.ContentType, documentStreamResult.FileName);
        }
    }
}
