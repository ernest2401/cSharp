using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public SexType Sex { get; set; }
        public DateTime Birthday { get; set; }
        public UserStatus Status { get; set; }
        public virtual ICollection<ReservedBook> ReservedBooks { get; set; }
    }
}
