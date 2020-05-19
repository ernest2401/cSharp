using System;
using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.DTO.Models.Account
{
    public class AccountRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public SexType Sex { get; set; }
        public DateTime Birthday { get; set; }
    }
}
