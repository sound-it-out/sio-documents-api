using SIO.IntegrationEvents.Documents;

namespace SIO.Documents.API.V1.Requests
{
    public record UploadRequest(IFormFile File, TranslationType TranslationType, string TranslationSubject);
}
