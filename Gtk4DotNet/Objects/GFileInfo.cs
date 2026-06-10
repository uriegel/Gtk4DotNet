using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class GFileInfo : GObject
{
    public GFileInfo() : base() { }

    public string? GetContentType()
        => _GetContentType(this).PtrToString(false);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_info_get_content_type", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetContentType(GFileInfo fileInfo);
}