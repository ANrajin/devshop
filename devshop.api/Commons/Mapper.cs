using AutoMapper;
using devshop.api.Books;

namespace devshop.api.Commons;

public sealed class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<BooksRequest, Book>()
            .ForMember(dest => dest.PublishedAt, 
                opt => opt.MapFrom(x => DateOnly.FromDateTime(x.PublishedAt)))
            .ReverseMap();

        CreateMap<Book, BooksResponse>()
            .ForMember(dest => dest.PublishedAt, 
                opt => opt.MapFrom(x => x.HumanizedPublishedDate()));
    }
}