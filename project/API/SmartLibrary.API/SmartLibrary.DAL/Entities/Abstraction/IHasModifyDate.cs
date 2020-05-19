using System;

namespace SmartLibrary.DAL.Entities.Abstraction
{
    /// <summary>
    /// Represents the interface for entities with modify date
    /// </summary>
    public interface IHasModifyDate
    {
        /// <summary>
        /// Gets or sets ModifyDate
        /// </summary>
        DateTime ModifyDate { get; set; }
    }
}
