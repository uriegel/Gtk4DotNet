using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class GValue
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_value_init", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Init(nint gvalue, GTypes type);

    public static string? GetString(nint gvalue)
        => Marshal.PtrToStringUTF8(_GetString(gvalue));

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_string", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetString(nint gvalue, string? text);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetString(nint gvalue);
    
    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetBool(nint gvalue, bool value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetBool(nint gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetInt(nint gvalue, int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetInt(nint gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_uint", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetUInt(nint gvalue, uint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_uint", CallingConvention = CallingConvention.Cdecl)]
    public extern static uint GetUInt(nint gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_double", CallingConvention = CallingConvention.Cdecl)]
    public extern static double GetDouble(nint gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_double", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetDouble(nint gvalue, double value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_float", CallingConvention = CallingConvention.Cdecl)]
    public extern static float GetFloat(nint gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_float", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetFloat(nint gvalue, float value);

    public static nint Allocate()
    {
        // Allocate the size of GValue. If you don't need to manipulate the structure in managed code,
        // you could simply use a size constant based on your GLib version.
        int size = Marshal.SizeOf(typeof(GValueStruct));
        IntPtr ptr = Marshal.AllocHGlobal(size);
        // Optionally, zero the allocated memory.
        for (int i = 0; i < size; i++)
            Marshal.WriteByte(ptr, i, 0);
        return ptr;
    }

    public static void Free(nint gvaluePtr)
    {
        Marshal.FreeHGlobal(gvaluePtr);
    }
}    
