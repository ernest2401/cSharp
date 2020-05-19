using FluentValidation;
using SmartLibrary.DTO.Models.Account;

namespace SmartLibrary.API.ValidatorModels
{
	public class AccountUpdateValidator : AbstractValidator<AccountUpdateDto>
	{
		public AccountUpdateValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Birthday).NotNull();
			RuleFor(x => x.FirstName).NotEmpty();
			RuleFor(x => x.PhoneNumber).NotEmpty();
			RuleFor(x => x.Sex).NotEmpty();
		}
	}
}
