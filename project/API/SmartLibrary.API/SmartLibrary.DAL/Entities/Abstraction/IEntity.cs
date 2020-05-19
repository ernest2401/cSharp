namespace SmartLibrary.DAL.Entities.Abstraction
{
    /// <summary>
    /// Represents the interface for entities
    /// </summary>
    /// <typeparam name="TKey">Type of the entity's Key</typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// Shows whether the entity is active
        /// </summary>
        bool IsActive { get; set; }
    }
}
