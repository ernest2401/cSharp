using AutoMapper;
using Moq;
using SmartLibrary.BLL.MapperProfiles;
using SmartLibrary.BLL.Services.Implementation;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DAL.Repostiry.Implementation;
using SmartLibrary.DTO.Models.AvailableBook;
using SmartLibrary.DTO.Models.Results;
using Xunit;

namespace SmartLibrary.Tests.Tests
{
    public class AvailableBookTests
    {
        private readonly IMapper _mapper;
        private Mock<GenericRepository<Book, int>> _bookRepository;
        private Mock<GenericRepository<AvailableBook, int>> _availableBookRepository;
        private Mock<GenericRepository<ReservedBook, int>> _reservedBookRepository;

        private AvailableBookDto availableBookDto;
        private AvailableBookNewDto availableBookNewDto;

        public AvailableBookTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AvailableBookProfile());
            });

            availableBookDto = new AvailableBookDto()
            {
                Id = 1,
                BookId = 1, 
                Count = 10,
                MaxTermDays = 100, 
                IsActive = true
            };

            availableBookNewDto = new AvailableBookNewDto()
            {
                BookId = 1,
                Count = 10,
                MaxTermDays = 100
            };

            _mapper = new Mapper(configuration);
            _bookRepository = new Mock<GenericRepository<Book, int>>(MockBehavior.Loose, new object[] { });
            _availableBookRepository = new Mock<GenericRepository<AvailableBook, int>>(MockBehavior.Loose, new object[] { });
            _reservedBookRepository = new Mock<GenericRepository<ReservedBook, int>>(MockBehavior.Loose, new object[] { });
        }

        [Fact]
        public async void AddAvailableBookTest()
        {
            // Arrange
            _bookRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Book());
            var mockAvailableBookService = new Mock<AvailableBookService>(MockBehavior.Loose, new object[] { _bookRepository.Object, null, null, _mapper });
            mockAvailableBookService.Setup(x => x.CreateAsync(It.IsAny<AvailableBookDto>()))
                           .ReturnsAsync(new SingleResult<AvailableBookDto> { Data = availableBookDto, IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockAvailableBookService.Object.AddAvailableBook(availableBookNewDto);

            // Assets
            Assert.IsType<OperationResult>(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async void DeleteAvailableBookTest()
        {
            // Arrange
            var mockAvailableBookService = new Mock<AvailableBookService>(MockBehavior.Loose, new object[] { null, _availableBookRepository.Object, _reservedBookRepository.Object, _mapper });
            mockAvailableBookService.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                           .ReturnsAsync(new OperationResult { IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockAvailableBookService.Object.DeleteAvailableBook(1);

            // Assets
            Assert.IsType<OperationResult>(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Success", result.Message);
        }
    }
}
