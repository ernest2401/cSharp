using System.Threading.Tasks;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.Book;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.BLL.Services.Abstraction
{
    public interface IBookService : IBaseService<Book, BookDto>
    {
        Task<OperationResult> AddBook(BookNewDto model);
        Task<SingleResult<BookDto>> GetBook(int id);
        Task<SingleResult<BookDto>> UpdateBook(BookUpdateDto model);
        Task<OperationResult> DeleteBook(int id);
        Task<CollectionResult<BookDto>> SearchBooks(BookSearchDto model);
    }
}
