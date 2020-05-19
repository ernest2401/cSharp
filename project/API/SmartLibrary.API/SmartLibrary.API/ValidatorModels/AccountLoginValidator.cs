using FluentValidation;
using SmartLibrary.DTO.Models.Account;

namespace SmartLibrary.API.ValidatorModels
{
	public class AccountLoginValidator : AbstractValidator<AccountLoginDto>
	{
		public AccountLoginValidator()
		{
			RuleFor(x => x.Email).EmailAddress();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}
