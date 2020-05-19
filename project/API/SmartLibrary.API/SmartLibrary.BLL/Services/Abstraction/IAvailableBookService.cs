using System.Threading.Tasks;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.AvailableBook;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.BLL.Services.Abstraction
{
    public interface IAvailableBookService : IBaseService<AvailableBook, AvailableBookDto>
    {
        Task<OperationResult> AddAvailableBook(AvailableBookNewDto model);
        Task<OperationResult> DeleteAvailableBook(int id);
        Task<CollectionResult<AvailableBookDto>> SearchAvailableBooks(AvailableBookSearchDto model);
    }
}
