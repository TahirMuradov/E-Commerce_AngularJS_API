namespace Shop.Application.ResultTypes.Abstract
{
   public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
