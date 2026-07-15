using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class VideoInfo : GObject
{
    public int Width { get => gst_discoverer_video_info_get_width(this); }
    public int Height { get => gst_discoverer_video_info_get_height(this); }
    public int PixelAspectRatioNumerator { get => gst_discoverer_video_info_get_par_num(this); }
    public int PixelAspectRatioDenominator { get => gst_discoverer_video_info_get_par_denom(this); }
    public double DisplayAspectRatio { get => (double)Width * PixelAspectRatioNumerator / (Height * PixelAspectRatioDenominator);  }
    internal VideoInfo(nint info)
    {
        SetInternalHandle(info);
        AutoDestroyed = true;
    }

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern int gst_discoverer_video_info_get_par_num(VideoInfo info);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern int gst_discoverer_video_info_get_par_denom(VideoInfo info);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern int gst_discoverer_video_info_get_height(VideoInfo info);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern int gst_discoverer_video_info_get_width(VideoInfo info);
}