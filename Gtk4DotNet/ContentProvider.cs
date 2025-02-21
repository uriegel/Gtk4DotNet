using System.Runtime.InteropServices;
using System.Text;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ContentProvider
{
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_content_provider_new_typed", CallingConvention = CallingConvention.Cdecl)]
    public extern static ContentProviderHandle NewString(GTypeHandle type, string text);

    public static ContentProviderHandle NewFileUris(string[] filePaths)
    {
        var uriListBytes = Encoding.UTF8.GetBytes(string.Join('\n', filePaths.Select(n => new Uri(n).AbsoluteUri)));
        var uriListPtr = Marshal.AllocHGlobal(uriListBytes.Length);
        Marshal.Copy(uriListBytes, 0, uriListPtr, uriListBytes.Length);
        using var gBytes = GBytes.New(uriListPtr, uriListBytes.Length);
        var res = NewBytes("text/uri-list", gBytes);
        Marshal.FreeHGlobal(uriListPtr);
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_content_provider_new_for_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static ContentProviderHandle NewBytes(string mime, BytesHandle bytes);
}

