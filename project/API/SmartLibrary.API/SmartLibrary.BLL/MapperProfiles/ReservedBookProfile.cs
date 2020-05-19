using AutoMapper;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.ReservedBook;

namespace SmartLibrary.BLL.MapperProfiles
{
    /// <summary>
    /// Represents configuration of AutoMapper
    /// </summary>
    public class ReservedBookProfile : Profile
    {
        /// <summary>
        /// Constructor of AutoMapper
        /// </summary>
        public ReservedBookProfile()
        {
            CreateMap<ReservedBook, ReservedBookDto>().ReverseMap();
            CreateMap<ReservedBookNewDto, ReservedBookDto>();
        }
    }
}
