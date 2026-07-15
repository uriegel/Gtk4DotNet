using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MediaControls : Widget
{
    public bool IsPlaying
    {
        get => GetPlaying(this);
        set => SetPlaying(this, value);
    }
    
    public void SetMediaStream(IMediaStream? stream)
        => SetMediaStream(this, stream?.GetRaw() ?? 0);

    public MediaControls() : base() { }

    public MediaControls(Builder builder, string? name = null) : base(builder, name) { }

    public MediaControls(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_media_controls_set_media_stream", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMediaStream(MediaControls contrls, nint stream);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_media_stream_set_playing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPlaying(MediaControls contrls, bool val);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_media_stream_get_playing", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetPlaying(MediaControls contrls);
}
