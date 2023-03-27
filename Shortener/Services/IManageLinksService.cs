using Shortener.Dtos;

namespace Shortener.Services
{
    public interface IManageLinksService
    {
        public Task<string> GenerateShortenedLink(ShortenedLinkDto shortenedLinkDto);
        public ShortenedLinkDto GetOriginalLink(string shortenedLinkGuid);
        public Task IncrementViews(ShortenedLinkDto dto);
        public Task<List<ShortenedLinkDto>> GetAllShortenedLinks();
    }
}
