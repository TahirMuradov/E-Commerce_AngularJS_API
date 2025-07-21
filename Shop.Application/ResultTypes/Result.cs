using Shop.Application.Abstraction.Services;
using Shop.Application.ResultTypes.Abstract;
using System.Net;

namespace Shop.Application.ResultTypes
{
    public class Result : IResult
    {
        public bool IsSuccess { get; }

        public string Message { get; }
        public List<string> Messages { get; }
        public HttpStatusCode StatusCode { get; }

        public string ResponseLangCode { get { return Thread.CurrentThread.CurrentCulture.Name; } }


   
        public Result(bool IsSuccess, string LangCode, HttpStatusCode statusCode)
        {


            this.IsSuccess = IsSuccess;
            StatusCode = statusCode;
            
        }
        public Result(bool IsSuccess, string LangCode, string message, HttpStatusCode statusCode) : this(IsSuccess, LangCode, statusCode)
        {
            Message = message;


        }
        public Result(bool IsSuccess, string LangCode, List<string> messages, HttpStatusCode statusCode) : this(IsSuccess, LangCode, statusCode)
        {
            Messages = messages;

        }
     
    }
}
