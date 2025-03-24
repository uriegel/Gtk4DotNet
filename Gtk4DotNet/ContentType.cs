using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ContentType
{
    public static string? Guess(string filename)
        => GuessContentType(filename, 0, 0, 0).PtrToString(true);

    [DllImport(Libs.LibGtk, EntryPoint = "g_content_type_get_icon", CallingConvention = CallingConvention.Cdecl)]
    public extern static IconHandle GetIcon(string contentType);

    [DllImport(Libs.LibGtk, EntryPoint = "g_content_type_guess", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GuessContentType(string filename, nint nil1,  nint nil2, nint nil3);
}
