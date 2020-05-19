using FluentValidation;
using SmartLibrary.DTO.Models.Book;

namespace SmartLibrary.API.ValidatorModels
{
	public class BookUpdateValidator : AbstractValidator<BookUpdateDto>
	{
		public BookUpdateValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Author).NotEmpty();
			RuleFor(x => x.Condition).NotEmpty();
			RuleFor(x => x.Description).NotEmpty();
			RuleFor(x => x.Genre).NotEmpty();
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Pages).NotEmpty().GreaterThan(0);
		}
	}
}
