using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class GstTagList
{
    public string? Get(string tag)
        => gst_tag_list_get_string(p, tag, out var res) ? res.PtrToString(true) : null;

    internal GstTagList(nint p) => this.p = p;

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl)]
    static extern bool gst_tag_list_get_string(nint list, string tag, out nint value);

    readonly nint p;
}
