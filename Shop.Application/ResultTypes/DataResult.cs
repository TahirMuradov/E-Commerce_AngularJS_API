using Shop.Application.ResultTypes.Abstract;
using System.Net;

namespace Shop.Application.ResultTypes
{
   public class DataResult<T> : Result, IDataResult<T>
    {
        public T Response { get; }


        public DataResult(T response, bool Issuccess, string message, HttpStatusCode statusCode) : base(Issuccess, message, statusCode)
        {
            Response = response;
        }
        public DataResult(T response, bool Issuccess, List<string> messages, HttpStatusCode statusCode) : base(Issuccess, messages, statusCode)
        {
            Response = response;
        }
        public DataResult(T response, bool success, HttpStatusCode statusCode) : base(success, statusCode)
        {
            Response = response;
        }
    }
}
