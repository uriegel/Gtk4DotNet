using System.Reflection;
using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class CssProvider
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static CssProviderHandle New();

    /// <summary>
    /// Loads css style from GResource. You have to call Application.RegisterResources before loading
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static CssProviderHandle LoadFromResource(this CssProviderHandle handle, string path)
        => handle.SideEffect(h => h._LoadFromResource(path));

    /// <summary>
    /// Loads a css style from .NET resource
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="resourceStylePath"></param>
    /// <returns></returns>
    public static CssProviderHandle FromResource(this CssProviderHandle handle, string resourceStylePath)
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
            var gbytes = GBytes.New(memIntPtr, styleResource.Length);
            Marshal.FreeHGlobal(memIntPtr);
            _LoadFromBytes(handle, gbytes);
        }
        return handle;
    }

    public static CssProviderHandle FromData(this CssProviderHandle handle, string data)
    {
        _LoadFromData(handle, data, 0, 0);
        return handle;         
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_resource", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadFromResource(this CssProviderHandle handle, string path);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadFromBytes(this CssProviderHandle handle, BytesHandle bytes);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_css_provider_load_from_data", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadFromData(this CssProviderHandle handle, string data, nint nil, nint nil2);
}

