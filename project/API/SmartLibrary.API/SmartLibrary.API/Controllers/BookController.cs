using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DTO.Models.Book;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Route("AddBook")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> AddBook([FromBody] BookNewDto model)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new OperationResult();
                badResult.Message = string.Join("; ", ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
                return new OkObjectResult(badResult);
            }

            var result = await _bookService.AddBook(model);

            return new OkObjectResult(result);
        }

        [HttpDelete]
        [Route("DeleteBook/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var result = await _bookService.DeleteBook(id);

            return new OkObjectResult(result);
        }

        [HttpGet]
        [Route("GetBook/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetBook([FromRoute] int id)
        {
            var result = await _bookService.GetBook(id);

            return new OkObjectResult(result);
        }

        [HttpPut]
        [Route("UpdateBook")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> UpdateBook([FromBody] BookUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new OperationResult();
                badResult.Message = string.Join("; ", ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
                return new OkObjectResult(badResult);
            }

            var result = await _bookService.UpdateBook(model);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("SearchBooks")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> SearchBooks([FromBody] BookSearchDto model)
        {
            var result = await _bookService.SearchBooks(model);

            return new OkObjectResult(result);
        }
    }
}