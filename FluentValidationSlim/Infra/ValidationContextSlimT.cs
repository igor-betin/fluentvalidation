using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentValidationSlim.Infra;

public class ValidationContextSlim<T> : ValidationContextSlim
{
    public ValidationContextSlim Wrapped { get; }

    public override string PropertyPath
    {
        get => Wrapped.PropertyPath;
        set => Wrapped.PropertyPath = value;
    }

    public override object PropertyValueRaw
    {
        get => Wrapped.PropertyValueRaw;
        set => Wrapped.PropertyValueRaw = value;
    }

    public T PropertyValue
    {
        get { return (T)PropertyValueRaw; }
        set { PropertyValueRaw = value; }
    }

    public ValidationContextSlim(T value)
    {
        Wrapped = new ValidationContextSlim
        {
            PropertyValueRaw = value
        };
    }

    public ValidationContextSlim(ValidationContextSlim wrapped)
    {
        Wrapped = wrapped;
    }

    public ValidationContextSlim(ValidationContextSlim wrapped, T value)
    {
        Wrapped = new ValidationContextSlim
        {
            PropertyValueRaw = value,
            PropertyPath = wrapped.PropertyPath
        };
    }

    public ValidationContextSlim<R> SpawnForChild<R>(Expression<Func<T, R>> getter)
    {
        var compiled = getter.Compile();
        R propValue = PropertyValue == null
            ? default
            : compiled(PropertyValue);

        string propName = null;
        switch (getter.Body)
        {
            case MemberExpression memberExpression:
                propName = memberExpression.Member.Name;
                break;

            default:
                propName = getter.Body.ToString();
                break;
        }

        var result = Wrapped.SpawnForChild(propName, propValue).ForType<R>();
        return result;
    }


    public IEnumerable<ValidationContextSlim<TElem>> SpawnForList<TElem>()
    {
        var idx = 0;
        if (PropertyValueRaw == null)
            yield break;

        foreach (var elem in (IEnumerable)PropertyValueRaw)
        {
            var result = Wrapped.SpawnForChild(idx, elem).ForType<TElem>();
            yield return result;
            idx++;
        }
    }

}