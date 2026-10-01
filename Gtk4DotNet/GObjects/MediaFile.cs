using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MediaFile : GObject, IMediaStream, IPaintable
{
    public MediaFile(string fileName)
    {
        var p = NewForFilename(fileName);
        SetInternalHandle(p);
        CheckDiagnostics();
    }
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_media_file_new_for_filename", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewForFilename(string fileName);

    nint IMediaStream.GetRaw() => GetInternalHandle();
    nint IPaintable.GetRaw() => GetInternalHandle();
}
