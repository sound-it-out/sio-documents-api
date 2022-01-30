using SIO.Domain.Documents.Projections;
using SIO.Domain.Documents.Projections.Managers;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Extensions
{
    public static class EntityFrameworkCoreStoreProjectorOptionsExtensions
    {
        public static void WithDomainProjections(this EntityFrameworkCoreStoreProjectorOptions options)
            => options.WithProjection<Document, DocumentProjectionManager, SIOStoreDbContext>(o => o.Interval = 300)
                .WithProjection<UserDocuments, UserDocumentsProjectionManager, SIOStoreDbContext>(o => o.Interval = 300);
    }
}
