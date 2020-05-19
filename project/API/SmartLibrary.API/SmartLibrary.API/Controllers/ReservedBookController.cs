using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DTO.Models.ReservedBook;

namespace SmartLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservedBookController : ControllerBase
    {
        private readonly IReservedBookService _reservedBookService;

        public ReservedBookController(IReservedBookService reservedBookService)
        {
            _reservedBookService = reservedBookService;
        }

        [HttpPost]
        [Route("ReserveBook")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ReserveBook([FromBody] ReservedBookNewDto model)
        {
            var result = await _reservedBookService.ReserveBook(model);

            return new OkObjectResult(result);
        }

        [HttpDelete]
        [Route("ReturnBook/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ReturnBook([FromRoute] int id)
        {
            var result = await _reservedBookService.ReturnBook(id);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("SearchReservedBooks")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SearchReservedBooks([FromBody] ReservedBookSearchDto model)
        {
            var result = await _reservedBookService.SearchReservedBooks(model);

            return new OkObjectResult(result);
        }
    }
}