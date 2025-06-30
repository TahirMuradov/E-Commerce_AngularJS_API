using System.Net;

namespace Shop.Application.ResultTypes.Concrete.SuccessResults
{
   public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data,  string LangCode,string message, HttpStatusCode statusCode) : base(data, true, LangCode, message, statusCode)
        {
        }

        public SuccessDataResult(T data, string LangCode, HttpStatusCode statusCode) : base(data, true,LangCode,  statusCode)
        {
        }

        public SuccessDataResult(string LangCode,string message, HttpStatusCode statusCode) : base(default, true,LangCode, message,  statusCode)
        {
        }
        public SuccessDataResult(List<string> messages, string LangCode, HttpStatusCode statusCode) : base(default, true,LangCode,messages ,statusCode)
        {
        }
        public SuccessDataResult(string LangCode,HttpStatusCode statusCode) : base(default,true,LangCode, statusCode)
        {
        }
    }
}
