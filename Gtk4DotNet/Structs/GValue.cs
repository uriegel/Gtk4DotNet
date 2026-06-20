using System.Runtime.InteropServices;

namespace Gtk4DotNet;

[StructLayout(LayoutKind.Sequential)]
public struct GValue
{
    public IntPtr g_type;
    public IntPtr data1;
    public IntPtr data2;

    public GValue(object? val)
    {
        if (val is string s)
        {
            Init(ref this, GTypes.String);
            SetString(ref this, s);
        }
        else if (val is bool b)
        {
            Init(ref this, GTypes.Boolean);
            SetBool(ref this, b);
        }
        else if (val is int n)
        {
            Init(ref this, GTypes.Int);
            SetInt(ref this, n);
        }
        else if (val is uint u)
        {
            Init(ref this, GTypes.UInt);
            SetUInt(ref this, u);
        }
        else if (val is double d)
        {
            Init(ref this, GTypes.Double);
            SetDouble(ref this, d);
        }
        else if (val is float f)
        {
            Init(ref this, GTypes.Float);
            SetFloat(ref this, f);
        }
        else
            Init(ref this, GTypes.String);
    }

    public GValue GetProperty(GObject obj, string name)
    {
        GetProperty(obj, name, ref this);
        return this;
    }

    public GValue Init(GTypes type)
    {
        Init(ref this, type);
        return this;
    } 

    public string? GetString() => Marshal.PtrToStringUTF8(GetString(ref this));
    public bool GetBool() => GetBool(ref this);
    public int GetInt() => GetInt(ref this);
    public uint GetUInt() => GetUInt(ref this);
    public double GetDouble() => GetDouble(ref this);
    public float GetFloat() => GetFloat(ref this);

    public void Unset() => Unset(ref this);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_init", CallingConvention = CallingConvention.Cdecl)]
    extern static void Init(ref GValue gvalue, GTypes type);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetString(ref GValue gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_string", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetString(ref GValue gvalue, string? text);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetBool(ref GValue gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBool(ref GValue gvalue, bool value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_int", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetInt(ref GValue gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_int", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetInt(ref GValue gvalue, int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_uint", CallingConvention = CallingConvention.Cdecl)]
    extern static uint GetUInt(ref GValue gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_uint", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetUInt(ref GValue gvalue, uint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_double", CallingConvention = CallingConvention.Cdecl)]
    extern static double GetDouble(ref GValue gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_double", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDouble(ref GValue gvalue, double value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_get_float", CallingConvention = CallingConvention.Cdecl)]
    extern static float GetFloat(ref GValue gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_set_float", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFloat(ref GValue gvalue, float value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_value_unset", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unset(ref GValue gvalue);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_property", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetProperty(GObject obj, string name, ref GValue value);
}







