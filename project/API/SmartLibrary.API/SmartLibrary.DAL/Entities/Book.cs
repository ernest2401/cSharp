using System;
using System.Collections.Generic;
using SmartLibrary.DAL.Entities.Abstraction;
using SmartLibrary.DAL.Enumeration;

namespace SmartLibrary.DAL.Entities
{
    public class Book : IEntity<int>, IHasCreationDate, IHasModifyDate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public BookGenre Genre { get; set; }
        public int Pages { get; set; }
        public string? ImageUrl { get; set; }
        public BookCondition Condition { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public virtual ICollection<AvailableBook> AvailableBooks { get; set; }
    }
}
