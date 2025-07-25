﻿
using Castle.DynamicProxy;
using FluentValidation;
using Shop.Application.Abstraction.Services;

namespace Shop.Infrastructure;

public class ValidationAspect : MethodInterception
{
    private Type _validatorType;

    public ValidationAspect(Type validatorType)
    {
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
        {
            throw new System.Exception("Exception");
        }

        _validatorType = validatorType;
      
    }
    protected override void OnBefore(IInvocation invocation)
    {

        var validator = (IValidator)Activator.CreateInstance(_validatorType);
     
        var entityType = _validatorType.BaseType.GetGenericArguments()[0];
        var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
        foreach (var entity in entities)
        {
            ValidationTool.Validate(validator, entity);
        }
    }
}
