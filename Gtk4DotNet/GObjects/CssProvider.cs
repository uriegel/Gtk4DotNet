using System.Reflection;
using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class CssProvider : GObject
{
    /// <summary>
    /// Loads a css style from .NET resource
    /// </summary>
    /// <param name="resourceStylePath"></param>
    /// <returns></returns>
    public static CssProvider FromResource(string resourceStylePath)
    {
        var styleResource = Assembly
            .GetEntryAssembly()
            ?.GetManifestResourceStream(resourceStylePath);
        if (styleResource != null)
        {
            var memIntPtr = Marshal.AllocHGlobal((int)styleResource.Length);
            unsafe
            {
                var memBytePtr = (byte*)memIntPtr.ToPointer();
                var writeStream = new UnmanagedMemoryStream(memBytePtr, styleResource.Length, styleResource.Length, FileAccess.Write);
                styleResource.CopyTo(writeStream);
            }
            using var gbytes = GBytes.New(memIntPtr, styleResource.Length);
            Marshal.FreeHGlobal(memIntPtr);
            var res = New();
            LoadFromBytes(res, gbytes);
            return res;
        }
        else
            throw new KeyNotFoundException($"Could not get CssProvicer fro resource: {resourceStylePath}");
    }

    public static CssProvider FromData(string data)
    {
        var res = New();
        LoadFromData(res, data, 0, 0);
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CssProvider New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_resource", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadFromResource(CssProvider handle, string path);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static void LoadFromBytes(CssProvider handle, GBytes bytes);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_data", CallingConvention = CallingConvention.Cdecl)]
    extern static void LoadFromData(CssProvider handle, string data, nint nil, nint nil2);
}
