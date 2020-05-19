using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.DTO.Models.Book
{
    public class BookSearchDto
    {
        public string? SearchString { get; set; }
        public BookGenre? Genre { get; set; }
        public BookCondition? Condition { get; set; }
    }
}
