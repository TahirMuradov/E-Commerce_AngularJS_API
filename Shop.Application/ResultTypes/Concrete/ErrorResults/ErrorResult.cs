using System.Net;

namespace Shop.Application.ResultTypes.Concrete.ErrorResults
{
  public  class ErrorResult : Result
    {
        public ErrorResult( string LangCode,List<string> messages, HttpStatusCode statusCode) : base(false, LangCode, messages, statusCode)
        {
        }
        public ErrorResult(string LangCode,string message, HttpStatusCode statusCode) : base(false,LangCode, message, statusCode)
        {
        }
        public ErrorResult(string LangCode,HttpStatusCode statusCode) : base(false,  LangCode,statusCode)
        {
        }
    }
}
