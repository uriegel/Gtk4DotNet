using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GFileInfo
{
    public static string? GetContentType(this FileInfoHandle fileInfo)
        => fileInfo._GetContentType().PtrToString(false);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_info_get_content_type", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetContentType(this FileInfoHandle fileInfo);
}