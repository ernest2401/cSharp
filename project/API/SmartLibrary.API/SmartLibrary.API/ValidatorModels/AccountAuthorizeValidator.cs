using FluentValidation;
using SmartLibrary.DTO.Models.Account;

namespace SmartLibrary.API.ValidatorModels
{
	public class AccountAuthorizeValidator : AbstractValidator<AccountAuthorizeDto>
	{
		public AccountAuthorizeValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Token).NotEmpty();
		}
	}
}
