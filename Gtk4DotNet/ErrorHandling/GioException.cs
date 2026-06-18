using Gtk4DotNet.ErrorHandling.ErrorCodes;

namespace Gtk4DotNet;

public class GioException : GtkException
{
    public bool TargetNotFound { get; }

    public new IO Code { get => (IO)base.Code; }
    
    internal GioException(GError error) : base(error) { }
    
    internal GioException(GError error, GFile file) : base(error)
    {
        if (file.Exists && error.Code == (int)IO.NotFound)
            TargetNotFound = true;
    }

    internal GioException(string domain, IO code, string message)  
        : base(domain, (int)code, message) {}
}