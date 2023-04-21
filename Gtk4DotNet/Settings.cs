using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Settings
{
    /// <summary>
    /// The caller of the function takes ownership of the data, and is responsible for freeing it.
    /// </summary>
    /// <param name="schemaId"></param>
    /// <returns></returns>
    [DllImport(Globals.LibGtk, EntryPoint = "g_settings_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string schemaId);

    [DllImport(Globals.LibGtk, EntryPoint="g_settings_bind", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Bind(IntPtr settings, string key, IntPtr obj, string property, BindFlags flags);
        
    [DllImport(Globals.LibGtk, EntryPoint="g_settings_set_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetBool(IntPtr obj, string name, bool value);

    [DllImport(Globals.LibGtk, EntryPoint="g_settings_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetBool(IntPtr obj, string name);

    [DllImport(Globals.LibGtk, EntryPoint="g_settings_set_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetInt(IntPtr obj, string name, int value);

    [DllImport(Globals.LibGtk, EntryPoint="g_settings_get_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetInt(IntPtr obj, string name);

    [DllImport(Globals.LibGtk, EntryPoint="g_settings_create_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr CreateAction(IntPtr obj, string key);
}

