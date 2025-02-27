using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class StringObject
{
    public static string? Get(this StringObjectHandle obj)
        => Marshal.PtrToStringUTF8(_Get(obj));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_string_object_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _Get(this StringObjectHandle obj);
}
