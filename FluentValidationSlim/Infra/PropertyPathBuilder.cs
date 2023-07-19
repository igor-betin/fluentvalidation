namespace FluentValidationSlim.Infra;

public static class PropertyPathBuilder
{
    public static string AppendPath(string oldPath, string newProp)
    {
        if (string.IsNullOrEmpty(newProp))
            return oldPath;
        if (string.IsNullOrEmpty(oldPath))
            return newProp;
        return oldPath + "." + newProp;
    }

    public static string AppendPath(string oldPath, int index)
    {
        return oldPath + "[" + index + "]";
    }
}