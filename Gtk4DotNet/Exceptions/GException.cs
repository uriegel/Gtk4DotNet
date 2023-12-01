namespace GtkDotNet;

public class GException : Exception 
{
    public uint Domain { get; }
    public int Code { get; }

    internal static GException New(GError error, string source, string destination)
        => error.Domain switch
        {
            236 or 232 => FileException.Create(error, source, destination),
            _ => new GException(error)
        };

    internal GException(GError error) : base(error.Message)
    {
        Domain = error.Domain;
        Code = error.Code;
    }
}
