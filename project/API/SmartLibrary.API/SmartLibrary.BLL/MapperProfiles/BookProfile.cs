using AutoMapper;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.Book;

namespace SmartLibrary.BLL.MapperProfiles
{
    /// <summary>
    /// Represents configuration of AutoMapper
    /// </summary>
    public class BookProfile : Profile
    {
        /// <summary>
        /// Constructor of AutoMapper
        /// </summary>
        public BookProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<BookNewDto, BookDto>();
            CreateMap<BookUpdateDto, BookDto>()
                 .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); 
        }
    }
}
