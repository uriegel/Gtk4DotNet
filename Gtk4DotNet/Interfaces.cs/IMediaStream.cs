using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public interface IMediaStream
{
    public bool IsPlaying
    {
        get => GetPlaying(GetRaw());
        set => SetPlaying(GetRaw(), value);
    }

    internal nint GetRaw();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_media_stream_set_playing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPlaying(nint stream, bool val);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_media_stream_get_playing", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetPlaying(nint stream);

}