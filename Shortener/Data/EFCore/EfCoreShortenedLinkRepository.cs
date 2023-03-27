using Shortener.Data;
using Shortener.Models;

namespace Shortener.Data.EFCore
{
    public class EfCoreShortenedLinkRepository : EfCoreRepository<ShortenedLink, ApplicationDbContext>
    {
        public EfCoreShortenedLinkRepository(ApplicationDbContext context) : base(context)
        {

        }

        public ShortenedLink GetByShortenedLink(string shortenedLink)
        {
            return context.shortenedLinks.SingleOrDefault(x => x.ShortLink == shortenedLink);
        }

    }
}