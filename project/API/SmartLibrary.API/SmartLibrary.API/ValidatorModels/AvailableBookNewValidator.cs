using FluentValidation;
using SmartLibrary.DTO.Models.AvailableBook;

namespace SmartLibrary.API.ValidatorModels
{
	public class AvailableBookNewValidator : AbstractValidator<AvailableBookNewDto>
	{
		public AvailableBookNewValidator()
		{
			RuleFor(x => x.BookId).NotEmpty();
			RuleFor(x => x.Count).NotEmpty().GreaterThan(0);
			RuleFor(x => x.MaxTermDays).NotEmpty().GreaterThan(0);
		}
	}
}
