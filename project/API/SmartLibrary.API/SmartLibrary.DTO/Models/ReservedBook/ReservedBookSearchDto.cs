using SmartLibrary.DTO.Models.Book;

namespace SmartLibrary.DTO.Models.ReservedBook
{
    public class ReservedBookSearchDto: BookSearchDto
    {
        public string? UserId { get; set; }
    }
}
