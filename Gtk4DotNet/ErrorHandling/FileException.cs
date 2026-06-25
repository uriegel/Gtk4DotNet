using Gtk4DotNet.ErrorHandling.ErrorCodes;

namespace Gtk4DotNet;

/// <summary>
/// Gio Exception
/// </summary>
public class FileException : GtkException
{
    /// <summary>
    /// The error code
    /// </summary>
    public new ErrorHandling.ErrorCodes.File Code { get => (ErrorHandling.ErrorCodes.File)base.Code; }

    internal FileException(GError error) : base(error) { }
}