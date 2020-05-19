using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DAL.Repostiry.Abstraction;
using SmartLibrary.DTO.Models.Results;
using SmartLibrary.DAL.Extensions;
using SmartLibrary.DTO.Models.AvailableBook;

namespace SmartLibrary.BLL.Services.Implementation
{
    public class AvailableBookService : BaseService<AvailableBook, AvailableBookDto>, IAvailableBookService
    {
        private readonly IGenericRepository<Book, int> _bookRepository;
        private readonly IGenericRepository<AvailableBook, int> _availableBookRepository;
        private readonly IGenericRepository<ReservedBook, int> _reservedBookRepository;
        private readonly IMapper _mapper;

        public AvailableBookService(IGenericRepository<Book, int> bookRepository, IGenericRepository<AvailableBook, int> availableBookRepository, IGenericRepository<ReservedBook, int> reservedBookRepository, IMapper mapper) : base(availableBookRepository, mapper)
        {
            _bookRepository = bookRepository;
            _availableBookRepository = availableBookRepository;
            _reservedBookRepository = reservedBookRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult> AddAvailableBook(AvailableBookNewDto model)
        {
            var result = new OperationResult();

            var book = await _bookRepository.GetAsync(model.BookId);
            if (book == null)
            {
                result.Message = "Book was not found";
                return result;
            }

            var addAvailableBookResult = await CreateAsync(_mapper.Map<AvailableBookNewDto, AvailableBookDto>(model));

            result.IsSuccessful = addAvailableBookResult.IsSuccessful;
            result.Message = addAvailableBookResult.Message;

            return result;
        }

        public async Task<OperationResult> DeleteAvailableBook(int id)
        {
            var result = new OperationResult();

            var reservedBooks = _reservedBookRepository.Get(x => x.AvailableBookId == id, true);
            if (reservedBooks.Count() != 0)
            {
                result.Message = "Available book is still using. Return all reserved books until delete";
                return result;
            }

            var deleteAvailableBookResult = await DeleteAsync(id);

            result.IsSuccessful = deleteAvailableBookResult.IsSuccessful;
            result.Message = deleteAvailableBookResult.Message;

            return result;
        }

        public async Task<CollectionResult<AvailableBookDto>> SearchAvailableBooks(AvailableBookSearchDto model)
        {
            var result = new CollectionResult<AvailableBookDto>();

            var entities = _availableBookRepository
                .Get()
                .WhereIf(model.Genre != null, item => item.Book.Genre == model.Genre)
                .WhereIf(model.Condition != null, item => item.Book.Condition == model.Condition)
                .WhereIf(!string.IsNullOrEmpty(model.SearchString),
                         x => x.Book.Name.ToLower().Contains(model.SearchString.ToLower()) ||
                         x.Book.Author.ToLower().Contains(model.SearchString.ToLower()))
                .Include(x => x.Book)
                .Include(x => x.ReservedBooks)
                .AsQueryable();

            var searchResult = await entities.ToListAsync();

            if (searchResult != null)
            {
                result.Items = _mapper.Map<List<AvailableBook>, List<AvailableBookDto>>(searchResult);
                result.IsSuccessful = true;
            }

            return result;
        }
    }
}
