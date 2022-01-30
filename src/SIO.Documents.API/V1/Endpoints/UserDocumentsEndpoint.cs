using Microsoft.AspNetCore.Mvc;
using SIO.Documents.API.Extensions;
using SIO.Documents.API.V1.Responses;
using SIO.Domain.Documents.Queries;
using SIO.Infrastructure;
using SIO.Infrastructure.Queries;
using System.Security.Claims;

namespace SIO.Documents.API.V1.Endpoints
{
    public static class UserDocumentsEndpoint
    {
        public static IEndpointRouteBuilder MapUserDocumentsEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("v1", GetUserDocumentsAsync)
                .RequireAuthorization();

            return builder;
        }

        private static async Task<IEnumerable<UserDocumentResponse>> GetUserDocumentsAsync([FromServices] IQueryDispatcher queryDispatcher,
            ClaimsPrincipal user,
            CancellationToken cancellationToken,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25)
        {
            var documentsResult = await queryDispatcher.DispatchAsync(new GetDocumentsForUserQuery(CorrelationId.New(), user.Actor(), page, pageSize), cancellationToken);
            return documentsResult.Documents.Select(d => new UserDocumentResponse(d.DocumentId, d.FileName));
        }
    }
}
