
using Castle.DynamicProxy;
using FluentValidation;

namespace Shop.Infrastructure;

public class ValidationAspect : MethodInterception
{
    private Type _validatorType;
    private object _param;
    public ValidationAspect(Type validatorType, object param)
    {
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
        {
            throw new System.Exception("Exception");
        }

        _validatorType = validatorType;
        _param = param;
    }
    protected override void OnBefore(IInvocation invocation)
    {
        var validator = (IValidator)Activator.CreateInstance(_validatorType,_param);
        var entityType = _validatorType.BaseType.GetGenericArguments()[0];
        var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
        foreach (var entity in entities)
        {
            ValidationTool.Validate(validator, entity);
        }
    }
}
