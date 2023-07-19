namespace FluentValidationSlim.Infra;

public class ValidationErrorSlim
{
    public ValidationErrorSlim()
    {
    }

    public ValidationErrorSlim(string propertyPath, string format, params object[] arguments)
    {
        Message = string.Format(format, arguments);
        PropertyPath = propertyPath;
    }

    public string PropertyPath { get; set; }

    public string Message { get; set; }
}