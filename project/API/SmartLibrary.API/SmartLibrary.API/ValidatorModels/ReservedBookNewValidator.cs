using FluentValidation;
using SmartLibrary.DTO.Models.ReservedBook;

namespace SmartLibrary.API.ValidatorModels
{
	public class ReservedBookNewValidator : AbstractValidator<ReservedBookNewDto>
	{
		public ReservedBookNewValidator()
		{
			RuleFor(x => x.AvailableBookId).NotEmpty();
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}
