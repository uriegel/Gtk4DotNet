using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class Variant : BaseHandle
{
    public bool AutoDestroyed { get; set; }

    public static bool GetBool(nint variant) => GetRawBool(variant) != 0;
    public static string GetString(nint variant) => GetRawString(variant, 0).PtrToString(false) ?? "";

    public static Variant New(string value, bool autoDestroyed = true)
    {
        var res = _New(value ?? "");
        res.AutoDestroyed = autoDestroyed;
        return res;
    }

    public static Variant New(bool value, bool autoDestroyed = true)
    {
        var res = NewBool(value ? -1 : 0);
        res.AutoDestroyed = autoDestroyed;
        return res;
    }

    public string GetString() => GetString(this, 0).PtrToString(false) ?? "";

    public bool GetBool() => GetBool(this) != 0;

    protected override bool ReleaseHandle()
    {
        if (!AutoDestroyed)
            Unref(handle);
        return true;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    extern static Variant _New(string value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static Variant NewBool(int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetString(Variant value, nint size);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetRawString(nint value, nint size);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetBool(Variant value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetRawBool(nint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unref(nint obj);
}