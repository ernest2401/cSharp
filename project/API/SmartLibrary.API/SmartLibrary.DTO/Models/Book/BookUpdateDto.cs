using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.DTO.Models.Book
{
    public class BookUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public BookGenre Genre { get; set; }
        public int Pages { get; set; }
        public string? ImageUrl { get; set; }
        public BookCondition Condition { get; set; }
    }
}
