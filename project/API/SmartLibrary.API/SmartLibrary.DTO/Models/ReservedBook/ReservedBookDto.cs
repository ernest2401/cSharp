using System;
using SmartLibrary.DTO.Models.Account;
using SmartLibrary.DTO.Models.AvailableBook;

namespace SmartLibrary.DTO.Models.ReservedBook
{
    public class ReservedBookDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AvailableBookId { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public AccountPublicDto User { get; set; }
        public AvailableBookDto AvailableBook { get; set; }
    }
}
