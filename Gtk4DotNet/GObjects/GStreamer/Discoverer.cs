using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

/// <summary>
/// The Discoverer is a utility object which allows to get as much information as possible from one or many URIs for a media stream.
/// </summary>
public class Discoverer : GObject
{
    /// <summary>
    /// Creates a new GStreamer Discoverer
    /// </summary>
    /// <param name="timeout">Timeout min value: 1s, max value: 1h</param>
    public Discoverer(TimeSpan timeout) : base()
    {
        var p = gst_discoverer_new((long)timeout.TotalMilliseconds * 1_000_000, out _);
        SetInternalHandle(p);
        CheckDiagnostics();
    }

    public DiscovererInfo? DiscoverUri(string uri)
    {
        var p = gst_discoverer_discover_uri(this, uri, out var err);
        if (p != 0 && err == 0)
        {
            var res = new DiscovererInfo();
            res.SetInternalHandle(p);
            res.CheckDiagnostics();
            return res;
        }
        else
            throw GtkException.Get(err, true);
    }

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern nint gst_discoverer_new(long timeout, out nint error);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern nint gst_discoverer_discover_uri(Discoverer discoverer, string uri, out nint error);
}