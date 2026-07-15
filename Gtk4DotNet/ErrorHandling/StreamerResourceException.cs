namespace Gtk4DotNet;

/// <summary>
/// Gio Exception
/// </summary>
public class StreamerResourceException : GtkException
{
    /// <summary>
    /// The error code
    /// </summary>
    public new ErrorHandling.ErrorCodes.StreamerResource Code { get => (ErrorHandling.ErrorCodes.StreamerResource)base.Code; }

    internal StreamerResourceException(GError error) : base(error) { }
}