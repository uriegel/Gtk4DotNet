using Gtk4DotNet.ErrorHandling;

namespace Gtk4DotNet;

public class GtkException : Exception
{
    public string Domain { get; }

    public int Code { get; }

    internal static Exception Get(nint gerror, bool free)
        => Get(GError.Get(gerror, free));

    internal static Exception Get(GError? error)
        => error?.Domain switch
        {
            Quarks.Gio => new GioException(error),
            null => new Exception("Unknown exception"),
            _ => new GtkException(error)
        };

    internal GtkException(GError error)
        : base(error.Message)
    {
        Domain = error.Domain;
        Code = error.Code;
    }

    internal GtkException(string domain, int code, string message)  
        : base(message)
    {
        Domain = domain;
        Code = code;
    }
}