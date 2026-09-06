using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// The Discoverer is a utility object which allows to get as much information as possible from one or many URIs for a media stream.
/// </summary>
public class DiscovererInfo : GObject
{
    public TimeSpan Duration { get => TimeSpan.FromMilliseconds(gst_discoverer_info_get_duration(this) / 1_000_000); }

    public VideoStreams GetVideoStreams() => new(gst_discoverer_info_get_video_streams(this));

    public GstTagList GetTagList() => new(gst_discoverer_info_get_tags(this));

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern long gst_discoverer_info_get_duration(DiscovererInfo info);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern nint gst_discoverer_info_get_video_streams(DiscovererInfo info);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern nint gst_discoverer_info_get_tags(DiscovererInfo info);
}
