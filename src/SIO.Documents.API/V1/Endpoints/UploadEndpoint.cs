using Microsoft.AspNetCore.Mvc;
using SIO.Documents.API.Extensions;
using SIO.Documents.API.V1.Requests;
using SIO.Domain.Documents.Commands;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using SIO.IntegrationEvents.Documents;
using System.Security.Claims;

namespace SIO.Documents.API.V1.Endpoints
{
    public static class UploadEndpoint
    {
        public static IEndpointRouteBuilder MapUploadEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("v1/upload", UploadAsync)
                .Accepts<UploadRequest>("multipart/form-data")
                .RequireAuthorization();

            return builder;
        }

        private static async Task<IResult> UploadAsync([FromServices] ICommandDispatcher commandDispatcher,
            HttpRequest request,
            ClaimsPrincipal user,
            CancellationToken cancellationToken)
        {
            var form = await request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();

            if (file is null
                || !form.TryGetValue("TranslationType", out var translatioTypeString)
                || int.TryParse(translatioTypeString, out var translatioType)
                || !form.TryGetValue("TranslationSubject", out var translationSubject))
                return Results.BadRequest();

            await commandDispatcher.DispatchAsync(new UploadDocumentCommand(Subject.New(), null, 1, user.Actor(), file, (TranslationType)translatioType, translationSubject), cancellationToken);
            return Results.Accepted();
        }
    }
}
