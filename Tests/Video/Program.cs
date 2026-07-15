using System.Runtime.InteropServices;
using Gtk4DotNet;

Streamer.Init();
var dst = gst_discoverer_new(1, out var e);
var info = gst_discoverer_discover_uri(dst, "file:///mnt/Home/uwe/B%C3%BCro.mkv", out var err); // GObject, TODO error handling
var dauert = gst_discoverer_info_get_duration(info);
var i = gst_discoverer_info_get_video_streams(info);
var glist = Marshal.PtrToStructure<GList>(i);
var p1 = gst_discoverer_video_info_get_par_num(glist.Data);
var p2 = gst_discoverer_video_info_get_par_denom(glist.Data);
var w = gst_discoverer_video_info_get_width(glist.Data);
var h = gst_discoverer_video_info_get_height(glist.Data);
var dar = (double)w * p1 / (h * p2);
//Streamer.Deinit();

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .WithAdditionals()
    .OnActivate(app =>
    {
        app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show();
    }
    ).Run();

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern nint gst_discoverer_new(long timeout, out nint error);

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern nint gst_discoverer_discover_uri(nint dissi, string uri, out nint error);

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern long gst_discoverer_info_get_duration(nint info);

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern nint gst_discoverer_info_get_video_streams(nint info);

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern int gst_discoverer_video_info_get_par_num(nint info);

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern int gst_discoverer_video_info_get_par_denom(nint info);

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern int gst_discoverer_video_info_get_height(nint info);

[DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl)]
static extern int gst_discoverer_video_info_get_width(nint info);

[StructLayout(LayoutKind.Sequential)]
struct GList {
    public nint Data;
    public nint Next;
    public nint Prev;
} 
