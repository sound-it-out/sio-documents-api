using Microsoft.AspNetCore.Mvc;
using SIO.Documents.API.Extensions;
using SIO.Documents.API.V1.Requests;
using SIO.Domain.Documents.Commands;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using System.Security.Claims;

namespace SIO.Documents.API.V1.Endpoints
{
    public static class UploadEndpoint
    {
        public static IEndpointRouteBuilder MapUploadEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("v1/upload", UploadAsync)
                .RequireAuthorization();

            return builder;
        }

        private static async Task<IResult> UploadAsync([FromServices] ICommandDispatcher commandDispatcher,
            [FromForm] UploadRequest request,
            ClaimsPrincipal user,
            CancellationToken cancellationToken)
        {
            await commandDispatcher.DispatchAsync(new UploadDocumentCommand(Subject.New(), null, 1, user.Actor(), request.File, request.TranslationType, request.TranslationSubject), cancellationToken);
            return Results.Accepted();
        }
    }
}
