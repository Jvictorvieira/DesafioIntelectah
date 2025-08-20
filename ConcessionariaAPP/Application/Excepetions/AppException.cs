namespace ConcessionariaAPP.Application.Excepetions;

public sealed class AppValidationException : Exception
{
    public AppValidationException(string field, string message)
        : base(message)
    {
        Data["Field"] = field;
        Data["ErrorMessage"] = message;
    }
}