namespace ConcessionariaAPP.Application.Excepetions;
public sealed class AppValidationException : Exception
{
    public Dictionary<string, List<string>> Errors { get; } = [];

    public AppValidationException Add(string field, string message)
    {
        if (!Errors.TryGetValue(field, out var list))
            Errors[field] = list = [];
        list.Add(message);
        return this;
    }
}