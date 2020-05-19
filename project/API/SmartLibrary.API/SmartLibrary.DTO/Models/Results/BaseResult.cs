using System;

namespace SmartLibrary.DTO.Models.Results
{
    /// <summary>
    /// Represents base result
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// Gets or sets status of operation
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the exception 
        /// </summary>
        public Exception Exception { get; set; }
    }
}
