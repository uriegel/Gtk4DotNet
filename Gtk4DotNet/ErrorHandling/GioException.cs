using Gtk4DotNet.ErrorHandling.ErrorCodes;

namespace Gtk4DotNet;

/// <summary>
/// Gio Exception
/// </summary>
public class GioException : GtkException
{
    /// <summary>
    /// When set to true and error code is <see cref="IO.Exists"/> then this specifies that the target path of a copy/move operation is not existing.
    /// </summary>
    public bool TargetNotFound { get; }

    /// <summary>
    /// The Gio error code
    /// </summary>
    public new IO Code { get => (IO)base.Code; }

    internal GioException(GError error) : base(error) { }

    internal GioException(GError error, GFile file) : base(error)
    {
        if (file.Exists && error.Code == (int)IO.NotFound)
            TargetNotFound = true;
    }

    internal GioException(string domain, IO code, string message)
        : base(domain, (int)code, message) { }
}