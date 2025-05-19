using System.Net;

namespace Shop.Application.ResultTypes.Concrete.ErrorResults
{
  public  class ErrorResult : Result
    {
        public ErrorResult(List<string> messages, HttpStatusCode statusCode) : base(false, messages, statusCode)
        {
        }
        public ErrorResult(string message, HttpStatusCode statusCode) : base(false, message, statusCode)
        {
        }
        public ErrorResult(HttpStatusCode statusCode) : base(false, statusCode)
        {
        }
    }
}
