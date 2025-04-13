using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Error
{
    public static string? GetMessage(this nint error)
        => error._GetMessage().PtrToString(false);

    [DllImport(Libs.LibGtk, EntryPoint = "g_error_get_message", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetMessage(this nint error);
}

