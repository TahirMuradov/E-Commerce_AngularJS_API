using System.Net;

namespace Shop.Application.ResultTypes.Concrete.ErrorResults
{
   public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, List<string> messages, HttpStatusCode statusCode) : base(data, false, messages, statusCode)
        {
        }
        public ErrorDataResult(T response, string message, HttpStatusCode statusCode) : base(response, false, message, statusCode)
        {
        }
        public ErrorDataResult(T response, HttpStatusCode statusCode) : base(response, false, statusCode)
        {
        }

        public ErrorDataResult(List<string> messages, HttpStatusCode statusCode) : base(default, false, messages, statusCode)
        {
        }
        public ErrorDataResult(string message, HttpStatusCode statusCode) : base(default, false, message, statusCode)
        {
        }
        public ErrorDataResult(HttpStatusCode statusCode) : base(default, false, statusCode)
        {
        }
    }
}
