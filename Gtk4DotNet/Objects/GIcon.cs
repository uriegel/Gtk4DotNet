using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class GIcon : GObject
{
    public static GIcon Get(string contentType)
    {
        var icon = _Get(contentType);
        icon.CheckDiagnostics();
        return icon;
    }

    public IEnumerable<string> ThemedNames()
    {
        var stringArray = GetNames(this);
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

    [DllImport(Libs.LibGtk, EntryPoint = "g_content_type_get_icon", CallingConvention = CallingConvention.Cdecl)]
    extern static GIcon _Get(string contentType);

    [DllImport(Libs.LibGtk, EntryPoint = "g_themed_icon_get_names", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetNames(GIcon icon);
}
