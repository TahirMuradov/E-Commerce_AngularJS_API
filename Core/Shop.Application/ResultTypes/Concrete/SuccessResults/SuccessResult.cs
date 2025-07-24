using System.Net;

namespace Shop.Application.ResultTypes.Concrete.SuccessResults
{
   public class SuccessResult : Result
    {
  
        public SuccessResult(string LangCode,HttpStatusCode statusCode) : base(true, LangCode,statusCode)
        {
        }
  
        public SuccessResult(string LangCode, string message, HttpStatusCode statusCode) : base(true, LangCode, message, statusCode)
        {
        }
        public SuccessResult(string LangCode,List<string> messages, HttpStatusCode statusCode) : base(true,  LangCode,messages, statusCode)
        {
        }
    }
}
