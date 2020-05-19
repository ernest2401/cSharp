using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DTO.Models.Account;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AccountRegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new OperationResult();
                badResult.Message = string.Join("; ", ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
                return new OkObjectResult(badResult);
            }

            var result = await _accountService.Register(model);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new OperationResult();
                badResult.Message = string.Join("; ", ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
                return new OkObjectResult(badResult);
            }

            var result = await _accountService.Login(model);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("Authorize")]
        public async Task<IActionResult> Authorize([FromBody] AccountAuthorizeDto model)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new OperationResult();
                badResult.Message = string.Join("; ", ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
                return new OkObjectResult(badResult);
            }

            var result = await _accountService.Authorize(model);

            return new OkObjectResult(result);
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var result = await _accountService.GetUser(id);

            return new OkObjectResult(result);
        }

        [HttpPut]
        [Route("UpdateUser")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateUser([FromBody] AccountUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                var badResult = new OperationResult();
                badResult.Message = string.Join("; ", ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage));
                return new OkObjectResult(badResult);
            }

            var result = await _accountService.UpdateUser(model);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("SearchUsers")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> SearchUsers([FromBody] AccountSearchDto model)
        {
            var result = await _accountService.SearchUsers(model);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("BlockUser/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> BlockUser([FromRoute] string id)
        {
            var result = await _accountService.BlockUser(id);

            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("UnBlockUser/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string id)
        {
            var result = await _accountService.UnBlockUser(id);

            return new OkObjectResult(result);
        }
    }
}