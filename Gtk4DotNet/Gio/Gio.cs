using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public static class Gio
{
    public static string? GuessContentType(string filename)
        => GuessContentType(filename, 0, 0, 0).PtrToString(true);

    [DllImport(Libs.LibGtk, EntryPoint = "g_content_type_guess", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GuessContentType(string filename, nint nil1, nint nil2, nint nil3);
}