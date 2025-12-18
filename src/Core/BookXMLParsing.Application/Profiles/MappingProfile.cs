using AutoMapper;
using BookXMLParsing.Application.DTO;
using BookXMLParsing.Domain.Entities;

namespace BookXMLParsing.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
