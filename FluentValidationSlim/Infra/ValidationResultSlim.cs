using System.Collections.Generic;
using System.Linq;

namespace FluentValidationSlim.Infra;

public class ValidationResultSlim
{
    public ValidationContextSlim Context { get; }

    // public ValidationResultSlim()
    // {
    //     Context = new ValidationContextSlim();
    // }

    public ValidationResultSlim()
    {
        Context = new ValidationContextSlim();
    }

    public ValidationResultSlim(ValidationContextSlim context)
    {
        Context = context;
    }

    public bool IsValid
    {
        get => !Errors.Any();
    }

    public List<ValidationErrorSlim> Errors { get; } = new List<ValidationErrorSlim>();

    /// <summary>
    /// Add validation error
    /// </summary>
    /// <param name="messageFormat">Error message template</param>
    /// <param name="values">Args for error message template</param>
    public void Error(string messageFormat, params object[] values)
    {
        var error = new ValidationErrorSlim
        {
            PropertyPath = Context.PropertyPath,
            Message = values.Any()
                ? string.Format(messageFormat, values)
                : messageFormat
        };

        Error(error);
    }

    /// <summary>
    /// Add validation error
    /// </summary>
    /// <param name="context">Child context</param>
    /// <param name="messageFormat">Error message template</param>
    /// <param name="values">Args for error message template</param>
    public void Error(ValidationContextSlim context, string messageFormat, params object[] values)
    {
        var error = new ValidationErrorSlim
        {
            PropertyPath = context.PropertyPath,
            Message = string.Format(messageFormat, values)
        };

        Error(error);
    }

    /// <summary>
    /// Add validation error
    /// </summary>
    /// <param name="propertyName">Overriden property name of child context</param>
    /// <param name="messageFormat">Error message template</param>
    /// <param name="values">Args for error message template</param>
    public void ErrorFor(string propertyName, string messageFormat, params object[] values)
    {
        var error = new ValidationErrorSlim
        {
            PropertyPath = PropertyPathBuilder.AppendPath(Context.PropertyPath, propertyName),
            Message = string.Format(messageFormat, values)
        };

        Error(error);
    }

    /// <summary>
    /// Add validation error
    /// </summary>
    /// <param name="context">Child context</param>
    /// <param name="propertyName">Overriden property name of child context</param>
    /// <param name="messageFormat">Error message template</param>
    /// <param name="values">Args for error message template</param>
    public void ErrorFor(ValidationContextSlim context, string propertyName, string messageFormat, params object[] values)
    {
        var error = new ValidationErrorSlim
        {
            PropertyPath = PropertyPathBuilder.AppendPath(context.PropertyPath, propertyName),
            Message = string.Format(messageFormat, values)
        };

        Error(error);
    }

    /// <summary>
    /// Merge other results to this one
    /// </summary>
    /// <returns>False if any new errors were added</returns>
    public bool Merge(params ValidationResultSlim[] others)
    {
        var newErrors = others.SelectMany(o => o.Errors).ToArray();
        if (!newErrors.Any())
            return true;
        Errors.AddRange(newErrors);
        return false;
    }

    /// <summary>
    /// Merge other results to this one, with prefix of parent path
    /// </summary>
    /// <returns>False if any new errors were added</returns>
    public bool Merge(string prefix, params ValidationResultSlim[] others)
    {
        var newErrors = others.SelectMany(o => o.Errors).ToArray();
        if (!newErrors.Any())
            return true;

        foreach (var error in newErrors)
            ErrorFor(PropertyPathBuilder.AppendPath(prefix, error.PropertyPath), error.Message);

        return false;
    }

    private void Error(ValidationErrorSlim error)
    {
        Errors.Add(error);
    }
}