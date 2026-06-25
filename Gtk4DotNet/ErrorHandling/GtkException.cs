using Gtk4DotNet.ErrorHandling;

namespace Gtk4DotNet;

/// <summary>
/// Base class of all Gtk exceptions
/// </summary>
public class GtkException : Exception
{
    /// <summary>
    /// The Gtk error domain
    /// </summary>
    public string Domain { get; }

    /// <summary>
    /// The error code that belongs to <see cref="Domain"/>
    /// </summary>
    public int Code { get; }

    internal static Exception Get(nint gerror, bool free)
        => Get(GError.Get(gerror, free));

    internal static Exception Get(GError? error)
        => error?.Domain switch
        {
            Quarks.Gio => new GioException(error),
            Quarks.File => new FileException(error),
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