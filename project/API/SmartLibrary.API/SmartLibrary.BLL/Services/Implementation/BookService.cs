using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DAL.Extensions;
using SmartLibrary.DAL.Repostiry.Abstraction;
using SmartLibrary.DTO.Models.Book;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.BLL.Services.Implementation
{
    public class BookService : BaseService<Book, BookDto>, IBookService
    {
        private readonly IGenericRepository<Book, int> _bookRepository;
        private readonly IGenericRepository<AvailableBook, int> _availableBookRepository;
        private readonly IMapper _mapper;

        public BookService(IGenericRepository<Book, int> bookRepository, IGenericRepository<AvailableBook, int> availableBookRepository, IMapper mapper) : base(bookRepository, mapper)
        {
            _bookRepository = bookRepository;
            _availableBookRepository = availableBookRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult> AddBook(BookNewDto model)
        {
            var result = new OperationResult();

            var addBookResult = await CreateAsync(_mapper.Map<BookNewDto, BookDto>(model));

            result.IsSuccessful = addBookResult.IsSuccessful;
            result.Message = addBookResult.Message;

            return result;
        }

        public async Task<OperationResult> DeleteBook(int id)
        {
            var result = new OperationResult();

            var availableBooks = _availableBookRepository.Get(x => x.BookId == id, true);
            if (availableBooks.Count() != 0)
            {
                result.Message = "Book is still using. Remove all available books until delete";
                return result;
            }

            var deleteBookResult = await DeleteAsync(id);
           
            result.IsSuccessful = deleteBookResult.IsSuccessful;
            result.Message = deleteBookResult.Message;

            return result;
        }

        public async Task<SingleResult<BookDto>> GetBook(int id)
        {
            var result = new SingleResult<BookDto>();

            var getBookResult = await GetAsync(id);

            result.IsSuccessful = getBookResult.IsSuccessful;
            result.Data = getBookResult.Data;
            result.Message = getBookResult.Message;

            return result;
        }

        public async Task<SingleResult<BookDto>> UpdateBook(BookUpdateDto model)
        {
            var result = new SingleResult<BookDto>();

            var updateBookResult = await UpdateAsync(_mapper.Map<BookUpdateDto, BookDto>(model));

            result.IsSuccessful = updateBookResult.IsSuccessful;
            result.Data = updateBookResult.Data;
            result.Message = updateBookResult.Message;

            return result;
        }

        public async Task<CollectionResult<BookDto>> SearchBooks(BookSearchDto model)
        {
            var result = new CollectionResult<BookDto>();

            var entities = _bookRepository
                .Get()
                .WhereIf(model.Genre != null, item => item.Genre == model.Genre)
                .WhereIf(model.Condition != null, item => item.Condition == model.Condition)
                .WhereIf(!string.IsNullOrEmpty(model.SearchString),
                         x => x.Name.ToLower().Contains(model.SearchString.ToLower()) ||
                         x.Author.ToLower().Contains(model.SearchString.ToLower()))
                .AsQueryable();

            var searchResult = await entities.ToListAsyncSafe();

            if (searchResult != null)
            {
                result.Items = _mapper.Map<List<Book>, List<BookDto>>(searchResult);
                result.IsSuccessful = true;
            }

            return result;
        }
    }
}
