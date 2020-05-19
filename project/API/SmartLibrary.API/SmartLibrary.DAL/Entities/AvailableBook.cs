using System.Collections.Generic;
using SmartLibrary.DAL.Entities.Abstraction;

namespace SmartLibrary.DAL.Entities
{
    public class AvailableBook : IEntity<int>
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Count { get; set; }
        public int MaxTermDays { get; set; }
        public bool IsActive { get; set; }
        public virtual Book Book { get; set; }
        public virtual ICollection<ReservedBook> ReservedBooks { get; set; }
    }
}
