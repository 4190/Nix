using AutoMapper;
using Shortener.Dtos;
using Shortener.Models;

namespace Shortener.MappingProfiles
{
    public class ShortenedLinkProfile : Profile
    {
        public ShortenedLinkProfile()
        {
            CreateMap<ShortenedLinkDto, ShortenedLink>().ReverseMap();
        }
    }
}
