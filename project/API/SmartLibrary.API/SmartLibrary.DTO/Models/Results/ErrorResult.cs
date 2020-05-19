using System;

namespace SmartLibrary.DTO.Models.Results
{
    public class ErrorResult
    {
        public ErrorResult()
        {
        }
        public ErrorResult(ArgumentException ex)
        {
            this.error = ex.Message + "-" + ex.StackTrace;
            this.param = ex.ParamName;
        }
        public ErrorResult(Exception ex)
        {
            this.error = ex.Message + "-" + ex.StackTrace;
        }
        public ErrorResult(string error, string param)
        {
            this.error = string.Format("{0}.{1}", param, error);
            this.param = param;
        }

        public string error { get; set; }
        public string param { get; set; }
    }
}
