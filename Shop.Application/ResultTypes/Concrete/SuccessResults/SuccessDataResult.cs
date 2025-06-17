using System.Net;

namespace Shop.Application.ResultTypes.Concrete.SuccessResults
{
   public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data,  string message, HttpStatusCode statusCode) : base(data, true,  message, statusCode)
        {
        }

        public SuccessDataResult(T data,  HttpStatusCode statusCode) : base(data, true,  statusCode)
        {
        }

        public SuccessDataResult(string message, HttpStatusCode statusCode) : base(default, true, message,  statusCode)
        {
        }
        public SuccessDataResult(List<string> messages, string LanguageCode, HttpStatusCode statusCode) : base(default, true,messages, statusCode)
        {
        }
        public SuccessDataResult( HttpStatusCode statusCode) : base(default, true,  statusCode)
        {
        }
    }
}
