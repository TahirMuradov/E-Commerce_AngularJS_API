using System.Net;

namespace Shop.Application.ResultTypes.Concrete.ErrorResults
{
   public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, string LangCode,List<string> messages, HttpStatusCode statusCode) : base(data, false,LangCode, messages, statusCode)
        {
        }
        public ErrorDataResult(T response, string LangCode,string message, HttpStatusCode statusCode) : base(response, false,LangCode, message, statusCode)
        {
        }
        public ErrorDataResult(T response,string LangCode,  HttpStatusCode statusCode) : base(response, false,LangCode, statusCode)
        {
        }

        public ErrorDataResult(string LangCode,List<string> messages, HttpStatusCode statusCode) : base(default, false,  LangCode,messages , statusCode)
        {
        }
        public ErrorDataResult(string LangCode,string message, HttpStatusCode statusCode) : base(default, false,LangCode,  message,  statusCode)
        {
        }
        public ErrorDataResult( string LangCode,HttpStatusCode statusCode) : base(default, false,  LangCode,statusCode)
        {
        }
    }
}
