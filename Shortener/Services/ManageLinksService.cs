using AutoMapper;
using Shortener.Data.EFCore;
using Shortener.Dtos;
using Shortener.Models;

namespace Shortener.Services
{
    public class ManageLinksService : IManageLinksService
    {
        private readonly EfCoreShortenedLinkRepository shortenedLinkRepository;
        private readonly IMapper _mapper;
        public ManageLinksService(EfCoreShortenedLinkRepository shortenedLinkRepository, IMapper mapper)
        {
            this.shortenedLinkRepository = shortenedLinkRepository;
            _mapper = mapper;
        }

        public async Task<string> GenerateShortenedLink(ShortenedLinkDto shortenedLinkDto)
        {
            string appUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";")[0]; //get base application url on local from launchSettings.json
            string guid;
            do
            {
                guid = Guid.NewGuid().ToString().Substring(0, 5);
                shortenedLinkDto.ShortLink = appUrl + "/" + guid;
            }
            while (GetOriginalLink(guid) != null);  //regenerate guid if found duplicate in DB

            ShortenedLink model = _mapper.Map<ShortenedLink>(shortenedLinkDto);
            await shortenedLinkRepository.Add(model);

            return appUrl + "/" + guid;
        }

        public ShortenedLinkDto GetOriginalLink(string shortenedLinkGuid)
        {
            string shortenedLink = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";")[0] + "/" + shortenedLinkGuid;

            return _mapper.Map<ShortenedLinkDto>(shortenedLinkRepository.GetByShortenedLink(shortenedLink));
        }

        public async Task<List<ShortenedLinkDto>> GetAllShortenedLinks()
        {
            List<ShortenedLink> allLinksInDb = await shortenedLinkRepository.GetAll();
            List<ShortenedLinkDto> allLinksDto = new List<ShortenedLinkDto>();
            foreach(ShortenedLink shortenedLink in allLinksInDb)
            {
                allLinksDto.Add(_mapper.Map<ShortenedLinkDto>(shortenedLink));
            }

            return allLinksDto;
        }

        public async Task IncrementViews(ShortenedLinkDto dto)
        {
            var model = _mapper.Map<ShortenedLink>(dto);
            model.Views += 1;

            await shortenedLinkRepository.Update(model);
        }
    }
}
