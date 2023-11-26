using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Settings
{
    /// <summary>
    /// The caller of the function takes ownership of the data, and is responsible for freeing it.
    /// </summary>
    /// <param name="schemaId"></param>
    /// <returns></returns>
    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SettingsHandle New(string schemaId);

    [DllImport(Libs.LibGtk, EntryPoint="g_settings_bind", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Bind(this SettingsHandle settings, string key, ObjectHandle obj, string property, BindFlags flags);
        
    [DllImport(Libs.LibGtk, EntryPoint="g_settings_bind", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Bind(this SettingsHandle settings, string key, IntPtr tag, string property, BindFlags flags);

    [DllImport(Libs.LibGtk, EntryPoint="g_settings_set_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetBool(this SettingsHandle settings, string name, bool value);

    [DllImport(Libs.LibGtk, EntryPoint="g_settings_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetBool(this SettingsHandle settings, string name);

    [DllImport(Libs.LibGtk, EntryPoint="g_settings_set_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetInt(this SettingsHandle settings, string name, int value);

    [DllImport(Libs.LibGtk, EntryPoint="g_settings_get_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetInt(this SettingsHandle settings, string name);

    [DllImport(Libs.LibGtk, EntryPoint="g_settings_create_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static ActionHandle CreateAction(this SettingsHandle settings, string key);
}

