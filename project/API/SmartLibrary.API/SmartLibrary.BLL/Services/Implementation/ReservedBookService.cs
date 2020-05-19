using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DAL.Extensions;
using SmartLibrary.DAL.Repostiry.Abstraction;
using SmartLibrary.DTO.Models.ReservedBook;
using SmartLibrary.DTO.Models.Results;
using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.BLL.Services.Implementation
{
    public class ReservedBookService : BaseService<ReservedBook, ReservedBookDto>, IReservedBookService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<AvailableBook, int> _availableBookRepository;
        private readonly IGenericRepository<ReservedBook, int> _reservedBookRepository;
        private readonly IMapper _mapper;

        public ReservedBookService(UserManager<ApplicationUser> userManager, IGenericRepository<AvailableBook, int> availableBookRepository, IGenericRepository<ReservedBook, int> reservedBookRepository, IMapper mapper) : base(reservedBookRepository, mapper)
        {
            _userManager = userManager;
            _availableBookRepository = availableBookRepository;
            _reservedBookRepository = reservedBookRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult> ReserveBook(ReservedBookNewDto model)
        {
            var result = new OperationResult();

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                result.Message = "User was not found";
                return result;
            }

            if (user.Status == UserStatus.Blocked)
            {
                result.Message = "User is not allowed to reserve a book";
                return result;
            }

            var availableBook = await _availableBookRepository.GetAsync(model.AvailableBookId);
            if (availableBook == null)
            {
                result.Message = "Available book was not found";
                return result;
            }

            var reservedBooks = _reservedBookRepository.Get(x => x.AvailableBookId == model.AvailableBookId, true);
            if (reservedBooks.Count() >= availableBook.Count)
            {
                result.Message = "All books are already reserved";
                return result;
            }

            var reserveBookResult = await CreateAsync(_mapper.Map<ReservedBookNewDto, ReservedBookDto>(model));

            result.IsSuccessful = reserveBookResult.IsSuccessful;
            result.Message = reserveBookResult.Message;

            return result;
        }

        public async Task<OperationResult> ReturnBook(int id)
        {
            var result = new OperationResult();

            var returnBookResult = await DeleteAsync(id);

            result.IsSuccessful = returnBookResult.IsSuccessful;
            result.Message = returnBookResult.Message;

            return result;
        }

        public async Task<CollectionResult<ReservedBookDto>> SearchReservedBooks(ReservedBookSearchDto model)
        {
            var result = new CollectionResult<ReservedBookDto>();

            var entities = _reservedBookRepository
                .Get()
                .WhereIf(model.UserId != null, item => item.UserId == model.UserId)
                .WhereIf(model.Genre != null, item => item.AvailableBook.Book.Genre == model.Genre)
                .WhereIf(model.Condition != null, item => item.AvailableBook.Book.Condition == model.Condition)
                .WhereIf(!string.IsNullOrEmpty(model.SearchString),
                         x => x.AvailableBook.Book.Name.ToLower().Contains(model.SearchString.ToLower()) ||
                         x.AvailableBook.Book.Author.ToLower().Contains(model.SearchString.ToLower()) ||
                         x.User.FirstName.ToLower().Contains(model.SearchString.ToLower()) || 
                         x.User.LastName.ToLower().Contains(model.SearchString.ToLower()) || 
                         x.User.Email.ToLower().Contains(model.SearchString.ToLower()))
                .Include(x => x.AvailableBook)
                .ThenInclude(x => x.Book)
                .AsQueryable();

            var searchResult = await entities.ToListAsync();

            if (searchResult != null)
            {
                result.Items = _mapper.Map<List<ReservedBook>, List<ReservedBookDto>>(searchResult);
                result.IsSuccessful = true;
            }

            return result;
        }
    }
}
