using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentValidationSlim.Infra;

public class ValidationContextSlim
{
    public virtual object PropertyValueRaw { get; set; }

    public virtual string PropertyPath { get; set; } = "";

    public ValidationContextSlim<T> ForType<T>()
    {
        return new ValidationContextSlim<T>(this);
    }

    public ValidationContextSlim<T> ForAsType<T>()
        where T : class
    {
        return new ValidationContextSlim<T>(this, PropertyValueRaw as T);
    }

    public ValidationContextSlim SpawnForChild(string propertyName, object propertyValue)
    {
        var result = new ValidationContextSlim
        {
            PropertyPath = PropertyPathBuilder.AppendPath(PropertyPath, propertyName),
            PropertyValueRaw = propertyValue
        };
        return result;
    }

    public ValidationContextSlim SpawnForChild(int index, object propertyValue)
    {
        var result = new ValidationContextSlim
        {
            PropertyPath = PropertyPathBuilder.AppendPath(PropertyPath, index),
            PropertyValueRaw = propertyValue
        };
        return result;
    }

    public static ValidationContextSlim<T> ForObject<T>(T o)
    {
        return new ValidationContextSlim<T>(o);
    }
}
