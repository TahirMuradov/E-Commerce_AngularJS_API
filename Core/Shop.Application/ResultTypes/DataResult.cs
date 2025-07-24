using Shop.Application.ResultTypes.Abstract;
using System.Net;

namespace Shop.Application.ResultTypes
{
   public class DataResult<T> : Result, IDataResult<T>
    {
        public T Data { get; }


        public DataResult(T data, bool Issuccess, string LangCode,string message, HttpStatusCode statusCode) : base(Issuccess, LangCode,message, statusCode)
        {
            Data = data;

        }
        public DataResult(T data, bool Issuccess,string LangCode, List<string> messages, HttpStatusCode statusCode) : base(Issuccess,LangCode, messages, statusCode)
        {
            Data = data;
        }
        public DataResult(T data, bool success,string LangCode, HttpStatusCode statusCode) : base(success,LangCode, statusCode)
        {
            Data = data;
        }
    }
}
