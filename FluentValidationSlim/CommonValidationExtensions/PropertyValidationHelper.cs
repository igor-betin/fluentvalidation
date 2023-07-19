using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using FluentValidationSlim.Infra;

namespace FluentValidationSlim.CommonValidationExtensions;

public static class PropertyValidationHelper
{
    public static bool NotEmptyWithMessage(
        this ValidationContextSlim context,
        ValidationResultSlim result,
        string message = null)
    {
        var value = context.PropertyValueRaw;
        if (IsNullOrEmpty(value))
        {
            result.Error(context, message ?? DefaultValidationMessages.FieldCanNotBeEmpty);
            return false;
        }

        return true;
    }

    public static ValidationResultSlim NotEmptyWithMessage(
        this ValidationContextSlim context,
        string message = null)
    {
        var result = new ValidationResultSlim(context);
        context.NotEmptyWithMessage(result, message);
        return result;
    }

    public static bool NotEmptyWithMessage<TParent, TProp>(
        this ValidationContextSlim<TParent> context,
        ValidationResultSlim parentResult,
        Expression<Func<TParent,TProp>> getter,
        string message = null)
    {
        var childContext = context.SpawnForChild(getter);
        var ok = NotEmptyWithMessage(childContext, parentResult, message);
        return ok;
    }
    
    public static bool IsNullOrEmpty(object propertyValue)
    {
        switch (propertyValue)
        {
            case null:
                return true;
            case string str:
                if (string.IsNullOrWhiteSpace(str))
                    return true;
                else
                    return false;
            case ICollection collection:
                if (collection.Count == 0 || propertyValue is Array array && array.Length == 0)
                    return true;
                else
                    return false;
            case IEnumerable source:
                if (source.Cast<object>().Any())
                    return false;
                else
                    return true;
        }
        return false;
    }
}