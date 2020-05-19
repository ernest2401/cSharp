using System.Collections.Generic;

namespace SmartLibrary.DTO.Models.Results
{
    /// <summary>
    /// Represents model for collection result
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class CollectionResult<TResult> : BaseResult
    {
        /// <summary>
        /// Gets or sets result message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets items
        /// </summary>
        public List<TResult> Items { get; set; }
    }
}
