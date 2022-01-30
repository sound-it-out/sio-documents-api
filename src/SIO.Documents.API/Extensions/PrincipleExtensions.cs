using IdentityModel;
using SIO.Infrastructure;
using System.Security.Claims;

namespace SIO.Documents.API.Extensions
{
    internal static class PrincipleExtensions
    {
        public static string Subject(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var claim = principal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);

            if (claim == null)
                throw new InvalidOperationException("sub claim is missing");

            return claim.Value;
        }

        public static Actor Actor(this ClaimsPrincipal principal)
            => SIO.Infrastructure.Actor.From(principal.Subject());
    }
}
