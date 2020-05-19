using System;
using System.Collections.Generic;
using SmartLibrary.DTO.Models.Results;

namespace SmartLibrary.BLL.Exceptions
{
    public class ApiException : Exception
    {
        private readonly List<ErrorResult> error;

        public ApiException(List<ErrorResult> error)
        {
            this.error = error;
        }

        public ApiException(List<ErrorResult> error, string message) : base(message)
        {
            this.error = error;
        }

        public ApiException(List<ErrorResult> error, string message, Exception inner) : base(message, inner)
        {
            this.error = error;
        }

        public List<ErrorResult> Error
        {
            get
            {
                return this.error;
            }
        }
    }
}
