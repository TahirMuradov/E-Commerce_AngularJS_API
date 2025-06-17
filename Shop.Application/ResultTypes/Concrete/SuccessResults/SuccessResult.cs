using System.Net;

namespace Shop.Application.ResultTypes.Concrete.SuccessResults
{
   public class SuccessResult : Result
    {
  
        public SuccessResult( string message, HttpStatusCode statusCode) : base(true,  message, statusCode)
        {
        }
        public SuccessResult(List<string> messages, HttpStatusCode statusCode) : base(true,  messages, statusCode)
        {
        }
        public SuccessResult( HttpStatusCode statusCode) : base(true, statusCode)
        {
        }
    }
}
