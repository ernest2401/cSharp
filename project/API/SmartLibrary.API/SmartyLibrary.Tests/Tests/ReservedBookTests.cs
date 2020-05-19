using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using SmartLibrary.BLL.MapperProfiles;
using SmartLibrary.BLL.Services.Implementation;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DAL.Repostiry.Implementation;
using SmartLibrary.DTO.Models.AvailableBook;
using SmartLibrary.DTO.Models.Book;
using SmartLibrary.DTO.Models.ReservedBook;
using SmartLibrary.DTO.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Xunit;

namespace SmartLibrary.Tests.Tests
{
    public class ReservedBookTests
    {
        private readonly IMapper _mapper;
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<GenericRepository<AvailableBook, int>> _availableBookRepository;
        private Mock<GenericRepository<ReservedBook, int>> _reservedBookRepository;

        private ReservedBookDto reservedBookDto;
        private ReservedBookNewDto reservedBookNewDto;

        public ReservedBookTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ReservedBookProfile());
            });

            reservedBookDto = new ReservedBookDto()
            {
                Id = 1,
                AvailableBookId = 1,
                UserId = "1"
            };

            reservedBookNewDto = new ReservedBookNewDto()
            {
                AvailableBookId = 1,
                UserId = "1"
            };

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();

            _mapper = new Mapper(configuration);
            _userManager = new Mock<UserManager<ApplicationUser>>(MockBehavior.Loose, new object[] { mockUserStore.Object, null, null, null, null, null, null, null, null });
            _availableBookRepository = new Mock<GenericRepository<AvailableBook, int>>(MockBehavior.Loose, new object[] { });
            _reservedBookRepository = new Mock<GenericRepository<ReservedBook, int>>(MockBehavior.Loose, new object[] { });
        }

        [Fact]
        public async void ReserveBookTest()
        {
            // Arrange
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                           .ReturnsAsync(new ApplicationUser());
            _availableBookRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
                           .ReturnsAsync(new AvailableBook());
            _reservedBookRepository.Setup(x => x.Get(It.IsAny<Expression<Func<ReservedBook, bool>>>(), It.IsAny<bool>()))
                           .Returns(new List<ReservedBook>().AsQueryable());
            var mockReservedBookService = new Mock<ReservedBookService>(MockBehavior.Loose, new object[] { _userManager.Object, _availableBookRepository.Object, _reservedBookRepository.Object, _mapper });
            mockReservedBookService.Setup(x => x.CreateAsync(It.IsAny<ReservedBookDto>()))
                           .ReturnsAsync(new SingleResult<ReservedBookDto> { Data = reservedBookDto, IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockReservedBookService.Object.ReserveBook(reservedBookNewDto);

            // Assets
            Assert.IsType<OperationResult>(result);
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public async void ReturnBookTest()
        {
            // Arrange
            var mockReservedBookService = new Mock<ReservedBookService>(MockBehavior.Loose, new object[] { null, null, null, _mapper });
            mockReservedBookService.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                           .ReturnsAsync(new OperationResult { IsSuccessful = true, Message = "Success" });

            // Act
            var result = await mockReservedBookService.Object.ReturnBook(1);

            // Assets
            Assert.IsType<OperationResult>(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Success", result.Message);
        }
    }
}
