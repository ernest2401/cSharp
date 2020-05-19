using System.Threading.Tasks;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.Account;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.BLL.Services.Abstraction
{
    public interface IAccountService
    {
        Task<OperationResult> Register(AccountRegistrationDto model);
        Task<SingleResult<AccountPublicDto>> Login(AccountLoginDto model);
        Task<SingleResult<AccountPublicDto>> Authorize(AccountAuthorizeDto model);
        Task<SingleResult<AccountPublicDto>> GetUser(string id);
        Task<SingleResult<AccountPublicDto>> UpdateUser(AccountUpdateDto model);
        Task<CollectionResult<AccountPublicDto>> SearchUsers(AccountSearchDto model);
        Task<OperationResult> BlockUser(string id);
        Task<OperationResult> UnBlockUser(string id);
    }
}
