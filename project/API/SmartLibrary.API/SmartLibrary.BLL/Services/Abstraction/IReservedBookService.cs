using System.Threading.Tasks;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.ReservedBook;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.BLL.Services.Abstraction
{
    public interface IReservedBookService : IBaseService<ReservedBook, ReservedBookDto>
    {
        Task<OperationResult> ReserveBook(ReservedBookNewDto model);
        Task<OperationResult> ReturnBook(int id);
        Task<CollectionResult<ReservedBookDto>> SearchReservedBooks(ReservedBookSearchDto model);
    }
}
