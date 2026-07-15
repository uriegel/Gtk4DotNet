namespace Gtk4DotNet.ErrorHandling.ErrorCodes;

/// <summary>
/// All GStreamer Resource error codes
/// </summary>
public enum StreamerResource
{
    Failed = 1,
    TooLazy,
    NotFound,
    Busy,
    OpenRead,
    OpenWrite,
    OpenReadWrite,
    Close,
    Read,
    Write,
    Seek,
    Sync,
    Settings,
    NoSpaceLeft,
    NotAuthorized,
    NumErrors
}