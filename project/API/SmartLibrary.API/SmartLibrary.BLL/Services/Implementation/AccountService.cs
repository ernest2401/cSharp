using AutoMapper;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Internal;

using SmartLibrary.DAL;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DTO.Models.Account;
using SmartLibrary.DTO.Models.Results;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.DAL.Enumeration;
using SmartLibrary.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SmartLibrary.BLL.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<OperationResult> Register(AccountRegistrationDto model)
        {
            var result = new OperationResult();

            var account = await _userManager.FindByEmailAsync(model.Email);

            if (account == null)
            {
                account = new ApplicationUser()
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Sex = model.Sex,
                    Birthday = model.Birthday,
                    Status = UserStatus.Active
                };

                var accountResult = await _userManager.CreateAsync(account, model.Password);

                if (accountResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(account.Email);
                    var rolesResult = await _userManager.AddToRoleAsync(user, "User");
                }

                result.IsSuccessful = accountResult.Succeeded;
                result.Message = accountResult.Errors.Select(x => x.Description).Join(";");
            }
            else
            {
                result.Message = "User with such email is already exists";
            }

            return result;
        }

        public async Task<SingleResult<AccountPublicDto>> Login(AccountLoginDto model)
        {
            var result = new SingleResult<AccountPublicDto>();

            var auth = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

            if (auth.Succeeded)
            {
                var user = await _userManager.Users.Include(x => x.ReservedBooks).FirstOrDefaultAsync(x => x.Email == model.Email);
                var roles = await GetUserRoles(user);
                var token = (string)await GenerateJwtToken(model.Email, user, roles);

                //await _userManager.SetAuthenticationTokenAsync(user, "SmartLibrary", "AuthorizationToken", token);
                _context.UserTokens.Add(new IdentityUserToken<string>()
                {
                    UserId = user.Id,
                    LoginProvider = "Identity" + Guid.NewGuid().ToString(),
                    Name = "Authorization token",
                    Value = token
                });
                _context.SaveChanges();

                result.Data = _mapper.Map<ApplicationUser, AccountPublicDto>(user);
                result.Data.AuthorizationToken = token;
                result.Data.Roles = roles;
                result.IsSuccessful = true;
            }
            else
            {
                result.Message = "Incorrect username or password";
            }

            return result;
        }

        public async Task<SingleResult<AccountPublicDto>> Authorize(AccountAuthorizeDto model)
        {
            var result = new SingleResult<AccountPublicDto>();

            var jwt = model.Token.Replace("Bearer ", "");
            var userToken = _context.UserTokens.FirstOrDefault(x => x.UserId == model.Id && x.Value == jwt);

            var isTokenLive = CheckTokenExpiration(jwt);
            if (!isTokenLive)
            {
                result.Message = "JWT token has been expired";
                return result;
            }

            var user = await _userManager.Users.Include(x => x.ReservedBooks).FirstOrDefaultAsync(x => x.Id == model.Id);
            var roles = await GetUserRoles(user);

            result.IsSuccessful = true;
            result.Data = _mapper.Map<ApplicationUser, AccountPublicDto>(user);
            result.Data.AuthorizationToken = jwt;
            result.Data.Roles = roles;

            return result;
        }

        public async Task<SingleResult<AccountPublicDto>> GetUser(string id)
        {
            var result = new SingleResult<AccountPublicDto>();

            var user = await _userManager.Users.Include(x => x.ReservedBooks).FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                result.IsSuccessful = true;
                result.Data = _mapper.Map<ApplicationUser, AccountPublicDto>(user);
            }
            else
            {
                result.Message = "User was not found";
            }


            return result;
        }

        public async Task<SingleResult<AccountPublicDto>> UpdateUser(AccountUpdateDto model)
        {
            var result = new SingleResult<AccountPublicDto>();

            var user = await _userManager.Users.Include(x => x.ReservedBooks).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Sex = model.Sex;
                user.Birthday = model.Birthday;
                user.PhoneNumber = model.PhoneNumber;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    result.IsSuccessful = true;
                    result.Data = _mapper.Map<ApplicationUser, AccountPublicDto>(user);
                }
                else
                {
                    result.Message = updateResult.Errors.Select(x => x.Description).Join(";");
                }
            }
            else
            {
                result.Message = "User was not found";
            }


            return result;
        }

        public async Task<CollectionResult<AccountPublicDto>> SearchUsers(AccountSearchDto model)
        {
            var result = new CollectionResult<AccountPublicDto>();

            var entities = _userManager
                .Users
                .WhereIf(model.Status != null, item => item.Status == model.Status)
                .Include(x => x.ReservedBooks)
                .AsQueryable();

            var searchResult = await entities.ToListAsync();

            if (searchResult != null)
            {
                result.Items = _mapper.Map<List<ApplicationUser>, List<AccountPublicDto>>(searchResult);
                result.IsSuccessful = true;
            }

            return result;
        }

        public async Task<OperationResult> BlockUser(string id)
        {
            var result = new OperationResult();

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Status = UserStatus.Blocked;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    result.IsSuccessful = true;
                }
                else
                {
                    result.Message = updateResult.Errors.Select(x => x.Description).Join(";");
                }
            }
            else
            {
                result.Message = "User was not found";
            }

            return result;
        }

        public async Task<OperationResult> UnBlockUser(string id)
        {
            var result = new OperationResult();

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Status = UserStatus.Active;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    result.IsSuccessful = true;
                }
                else
                {
                    result.Message = updateResult.Errors.Select(x => x.Description).Join(";");
                }
            }
            else
            {
                result.Message = "User was not found";
            }

            return result;
        }

        private async Task<object> GenerateJwtToken(string email, ApplicationUser user, List<string> roles)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("UserId", user.Id));

            if (_userManager.SupportsUserRole)
            {
                foreach (var roleName in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool CheckTokenExpiration(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var jwthandler = new JwtSecurityTokenHandler();
                var jwttoken = jwthandler.ReadToken(token);
                var expDate = jwttoken.ValidTo;
                return expDate > DateTime.UtcNow.AddMinutes(1);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            return roles.ToList();
        }
    }
}
