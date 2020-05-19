using System;
using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.DTO.Models.Account
{
    public class AccountUpdateDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public SexType Sex { get; set; }
        public DateTime Birthday { get; set; }
    }
}
