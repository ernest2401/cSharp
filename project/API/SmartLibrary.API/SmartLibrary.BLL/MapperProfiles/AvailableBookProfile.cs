using AutoMapper;
using System.Linq;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.AvailableBook;

namespace SmartLibrary.BLL.MapperProfiles
{
    /// <summary>
    /// Represents configuration of AutoMapper
    /// </summary>
    public class AvailableBookProfile : Profile
    {
        /// <summary>
        /// Constructor of AutoMapper
        /// </summary>
        public AvailableBookProfile()
        {
            CreateMap<AvailableBookDto, AvailableBook>();
            CreateMap<AvailableBook, AvailableBookDto>()
                .ForMember(dest => dest.ReservedBooksCount, opt => opt.MapFrom(src => src.ReservedBooks.Where(x => x.IsActive == true).Count()));
            CreateMap<AvailableBookNewDto, AvailableBookDto>();
        }
    }
}
