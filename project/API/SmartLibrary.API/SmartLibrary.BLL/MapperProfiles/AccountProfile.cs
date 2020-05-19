using AutoMapper;
using System.Linq;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.Account;

namespace SmartLibrary.BLL.MapperProfiles
{
    /// <summary>
    /// Represents configuration of AutoMapper
    /// </summary>
    public class AccountProfile : Profile
    {
        /// <summary>
        /// Constructor of AutoMapper
        /// </summary>
        public AccountProfile()
        {
            CreateMap<AccountPublicDto, ApplicationUser>();
            CreateMap<ApplicationUser, AccountPublicDto>()
                .ForMember(dest => dest.ReservedBooksCount, opt => opt.MapFrom(src => src.ReservedBooks.Where(x => x.IsActive == true).Count()));
            CreateMap<ApplicationUser, AccountUpdateDto>().ReverseMap();
        }
    }
}
