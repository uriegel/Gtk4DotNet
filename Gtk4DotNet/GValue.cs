using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class GValue
{
    public static string? GetString(nint gvalue)
        => Marshal.PtrToStringUTF8(_GetString(gvalue));

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_string", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetString(nint gvalue, string? text);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetString(nint gvalue);

}
