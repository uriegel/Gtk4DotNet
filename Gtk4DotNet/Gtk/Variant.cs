using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class Variant : BaseHandle
{
    public bool AutoDestroyed { get; set; }

    public static bool GetBool(nint variant) => GetRawBool(variant) != 0;
    public static string GetString(nint variant) => GetRawString(variant, 0).PtrToString(false) ?? "";

    public Variant(string value, bool autoDestroyed = true)
    {
        SetInternalHandle(New(value ?? ""));
        AutoDestroyed = autoDestroyed;
    }

    public Variant(bool value, bool autoDestroyed = true)
    {
        SetInternalHandle(NewBool(value ? -1 : 0));
        AutoDestroyed = autoDestroyed;
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
    extern static nint New(string value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewBool(int value);

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