using System.Reflection;
using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class CssProvider : GObject
{
    public static CssProvider New() => _New();

    /// <summary>
    /// Loads a css style from .NET resource
    /// </summary>
    /// <param name="resourceStylePath"></param>
    /// <returns></returns>
    public CssProvider FromResource(string resourceStylePath)
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
            LoadFromBytes(this, gbytes);
        }
        return this;
    }

    public CssProvider FromData(string data)
    {
        _LoadFromData(this, data, 0, 0);
        return this;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CssProvider _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_resource", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadFromResource(CssProvider handle, string path);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static void LoadFromBytes(CssProvider handle, GBytes bytes);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_data", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadFromData(CssProvider handle, string data, nint nil, nint nil2);
}
