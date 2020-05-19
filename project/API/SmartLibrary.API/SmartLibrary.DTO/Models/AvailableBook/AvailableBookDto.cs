using SmartLibrary.DTO.Models.Book;

namespace SmartLibrary.DTO.Models.AvailableBook
{
    public class AvailableBookDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Count { get; set; }
        public int MaxTermDays { get; set; }
        public bool IsActive { get; set; }
        public int ReservedBooksCount { get; set; }
        public BookDto Book { get; set; }
    }
}
