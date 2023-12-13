namespace GtkDotNet;

public class GError
{
    public uint Domain { get; }
    public int Code { get; }

    internal static GError New(GErrorStruct error, string source)
        => error.Domain switch
        {
            236 or 232 => FileError.Create(error, source),
            _ => new GError(error)
        };

    internal GError(GErrorStruct error)
    {
        Domain = error.Domain;
        Code = error.Code;
    }
}
