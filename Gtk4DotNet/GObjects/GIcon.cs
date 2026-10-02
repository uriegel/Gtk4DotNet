using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class GIcon : GObject
{
    public static GIcon? FromContentType(string contentType)
    {
        var handle = Get(contentType);
        if (handle == 0)
            return null;
        var res = new GIcon();
        res.SetInternalHandle(handle);
        res.CheckDiagnostics();
        return res;
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
    extern static nint Get(string contentType);

    [DllImport(Libs.LibGtk, EntryPoint = "g_themed_icon_get_names", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetNames(GIcon icon);
}
