using System;
using System.Collections.Generic;
using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.DTO.Models.Account
{
    public class AccountPublicDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public SexType Sex { get; set; }
        public DateTime Birthday { get; set; }
        public UserStatus Status { get; set; }
        public string AuthorizationToken { get; set; }
        public List<string> Roles { get; set; }
        public int ReservedBooksCount { get; set; }
    }
}
