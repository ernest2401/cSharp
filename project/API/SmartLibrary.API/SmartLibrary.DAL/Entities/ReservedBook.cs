using System;
using SmartLibrary.DAL.Entities.Abstraction;

namespace SmartLibrary.DAL.Entities
{
    public class ReservedBook : IEntity<int>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AvailableBookId { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsActive { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual AvailableBook AvailableBook { get; set; }
    }
}
