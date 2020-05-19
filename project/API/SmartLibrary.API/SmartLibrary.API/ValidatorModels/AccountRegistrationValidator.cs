using FluentValidation;
using SmartLibrary.DTO.Models.Account;

namespace SmartLibrary.API.ValidatorModels
{
	public class AccountRegistrationValidator : AbstractValidator<AccountRegistrationDto>
	{
		public AccountRegistrationValidator()
		{
			RuleFor(x => x.Birthday).NotNull();
			RuleFor(x => x.Email).EmailAddress();
			RuleFor(x => x.FirstName).NotEmpty();
			RuleFor(x => x.Password).NotEmpty();
			RuleFor(x => x.PhoneNumber).NotEmpty();
			RuleFor(x => x.Sex).NotEmpty();
		}
	}
}
