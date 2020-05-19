using AutoMapper;
using Moq;
using SmartLibrary.BLL.MapperProfiles;
using SmartLibrary.BLL.Services.Implementation;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DAL.Repostiry.Implementation;
using SmartLibrary.DTO.Models.Book;
using SmartLibrary.DTO.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace SmartLibrary.Tests.Tests
{
    public class BookServiceTests
    {
        private readonly IMapper _mapper;
        private Mock<GenericRepository<Book, int>> _bookRepository;
        private Mock<GenericRepository<AvailableBook, int>> _availableBookRepository;

        private Book book;
        private BookDto bookDto;
        private BookNewDto bookNewDto;
        private BookUpdateDto bookUpdateDto;

        public BookServiceTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BookProfile());
            });

            book = new Book()
            {
                Id = 1,
                Name = "Test Name",
                Description = "Test Description",
                Author = "Test Author",
                Genre = DAL.Enumeration.BookGenre.Adventure,
                Pages = 100,
                Condition = DAL.Enumeration.BookCondition.New,
                IsActive = true,
                CreationDate = new System.DateTime(),
                ModifyDate = new System.DateTime()
            };

            bookDto = new BookDto()
            {
                Id = 1,
                Name = "Test Name",
                Description = "Test Description",
                Author = "Test Author",
                Genre = DAL.Enumeration.BookGenre.Adventure,
                Pages = 100,
                Condition = DAL.Enumeration.BookCondition.New,
                IsActive = true
            };

            bookNewDto = new BookNewDto()
            {
                Name = "Test Name",
                Description = "Test Description",
                Author = "Test Author",
                Genre = DAL.Enumeration.BookGenre.Adventure,
                Pages = 100,
                Condition = DAL.Enumeration.BookCondition.New
            };

            bookUpdateDto = new BookUpdateDto()
            {
                Id = 1,
                Name = "Test Name",
                Description = "Test Description",
                Author = "Test Author",
                Genre = DAL.Enumeration.BookGenre.Adventure,
                Pages = 100,
                Condition = DAL.Enumeration.BookCondition.New
            };

            _mapper = new Mapper(configuration);
            _bookRepository = new Mock<GenericRepository<Book, int>>(MockBehavior.Loose, new object[] { });
            _availableBookRepository = new Mock<GenericRepository<AvailableBook, int>>(MockBehavior.Loose, new object[] { });
        }

        [Fact]
        public async void AddBookTest()
        {
            // Arrange
            var mockBookService = new Mock<BookService>(MockBehavior.Loose, new object[] { null, null, _mapper });
            mockBookService.Setup(x => x.CreateAsync(It.IsAny<BookDto>()))
                           .ReturnsAsync(new SingleResult<BookDto> { Data = bookDto, IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockBookService.Object.AddBook(bookNewDto);

            // Assets
            Assert.IsType<OperationResult>(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async void DeleteBookTest()
        {
            // Arrange
            var mockBookService = new Mock<BookService>(MockBehavior.Loose, new object[] { null, _availableBookRepository.Object, _mapper });
            mockBookService.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                           .ReturnsAsync(new OperationResult { IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockBookService.Object.DeleteBook(1);

            // Assets
            Assert.IsType<OperationResult>(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async void GetBookTest()
        {
            // Arrange
            var mockBookService = new Mock<BookService>(MockBehavior.Loose, new object[] { null, null, _mapper });
            mockBookService.Setup(x => x.GetAsync(It.IsAny<int>()))
                           .ReturnsAsync(new SingleResult<BookDto> { Data = bookDto, IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockBookService.Object.GetBook(1);

            // Assets
            Assert.IsType<SingleResult<BookDto>>(result);
            Assert.IsType<BookDto>(result.Data);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async void UpdateBookTest()
        {
            // Arrange
            var mockBookService = new Mock<BookService>(MockBehavior.Loose, new object[] { null, null, _mapper });
            mockBookService.Setup(x => x.UpdateAsync(It.IsAny<BookDto>()))
                           .ReturnsAsync(new SingleResult<BookDto> { Data = bookDto, IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockBookService.Object.UpdateBook(bookUpdateDto);

            // Assets
            Assert.IsType<SingleResult<BookDto>>(result);
            Assert.IsType<BookDto>(result.Data);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async void SearchBooksTest()
        {
            // Arrange
            _bookRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Book, bool>>>(), It.IsAny<bool>()))
                           .Returns(new List<Book>() { new Book () }.AsQueryable());
            var mockBookService = new Mock<BookService>(MockBehavior.Loose, new object[] { _bookRepository.Object, null, _mapper });

            // Act
            var result = await mockBookService.Object.SearchBooks(new BookSearchDto());

            // Assets
            Assert.IsType<CollectionResult<BookDto>>(result);
            Assert.IsType<List<BookDto>>(result.Items);
            Assert.True(result.IsSuccessful);
        }
    }
}
