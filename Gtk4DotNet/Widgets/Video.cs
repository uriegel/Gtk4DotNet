using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Video : Widget
{
    public bool AutoPlay
    {
        get => GetAutoPlay(this);
        set => SetAutoPlay(this, value);
    }

    public void SetFileName(string file) => SetFileName(this, file);
    public Video() : base() { }

    public Video(Builder builder, string? name = null) : base(builder, name) { }

    public Video(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }


    [DllImport(Libs.LibGtk, EntryPoint = "gtk_video_set_filename", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFileName(Video video, string file);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_video_get_autoplay", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetAutoPlay(Video video);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_video_set_autoplay", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetAutoPlay(Video video, bool value);
}
