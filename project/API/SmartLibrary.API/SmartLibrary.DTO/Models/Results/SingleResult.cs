namespace SmartLibrary.DTO.Models.Results
{
    /// <summary>
    /// Represents model for single result
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class SingleResult<TResult> : BaseResult
    {
        /// <summary>
        /// Gets or sets data of result
        /// </summary>
        public TResult Data { get; set; }

        /// <summary>
        /// Gets or sets message
        /// </summary>
        public string Message { get; set; }
    }
}
