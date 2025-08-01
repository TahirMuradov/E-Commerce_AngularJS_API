﻿using System.Net;

namespace Shop.Application.ResultTypes.Abstract
{
  public  interface IResult
    {
        public bool IsSuccess { get; }
        public string ResponseLangCode { get; }
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public List<string> Messages { get; }
    }
}
