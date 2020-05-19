using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DTO.Models.AvailableBook;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailableBookController : ControllerBase
    {
        private readonly IAvailableBookService _availableBookService;

        public AvailableBookController(IAvailableBookService availableBookService)
        {
            _availableBookService = availableBookService;
        }

        [HttpPost]
        [Route("AddAvailableBook")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> AddAvailableBook([FromBody] AvailableBookNewDto model)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new OperationResult();
                badResult.Message = string.Join("; ", ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
                return new OkObjectResult(badResult);
            }

            var result = await _availableBookService.AddAvailableBook(model);

            return new OkObjectResult(result);
        }

        [HttpDelete]
        [Route("DeleteAvailableBook/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> DeleteAvailableBook([FromRoute] int id)
        {
            var result = await _availableBookService.DeleteAvailableBook(id);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("SearchAvailableBooks")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SearchBooks([FromBody] AvailableBookSearchDto model)
        {
            var result = await _availableBookService.SearchAvailableBooks(model);

            return new OkObjectResult(result);
        }
    }
}