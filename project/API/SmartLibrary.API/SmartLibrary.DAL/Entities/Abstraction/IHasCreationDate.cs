using System;

namespace SmartLibrary.DAL.Entities.Abstraction
{
    /// <summary>
    /// Represents the interface for entities with creation date
    /// </summary>
    public interface IHasCreationDate
    {
        /// <summary>
        /// Gets or sets CreationDate
        /// </summary>
        DateTime CreationDate { get; set; }
    }
}
