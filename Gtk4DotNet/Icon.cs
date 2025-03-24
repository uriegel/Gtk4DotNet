using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Icon
{
    public static string[] Names(this IconHandle icon)
        => _IconToString(icon).PtrToString(true)?.Split(' ') ?? [];

    public static IEnumerable<string> ThemedNames(this IconHandle icon)
    {
        var stringArray = _ThemedGetNames(icon);
        int idx = 0;
        while (true)
        {
            var res = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(stringArray, idx));
            if (res != null)
                yield return res;
            else
                yield break;
            idx += 8;
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_icon_to_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _IconToString(IconHandle icon);

    [DllImport(Libs.LibGtk, EntryPoint = "g_themed_icon_get_names", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _ThemedGetNames(IconHandle icon);    
}