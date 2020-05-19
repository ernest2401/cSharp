using System.Collections.Generic;

namespace SmartLibrary.DTO.Models.Results
{
    /// <summary>
    /// Repcesents class for paged result dto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// Gets or sets total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="T"/> items
        /// </summary>
        public List<T> Items { get; set; }
    }
}
