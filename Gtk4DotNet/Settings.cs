using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Settings
{
    /// <summary>
    /// The caller of the function takes ownership of the data, and is responsible for freeing it.
    /// </summary>
    /// <param name="schemaId"></param>
    /// <returns></returns>
    public static SettingsHandle New(string schemaId)
        => _New(schemaId).SideEffect(res => res.SchemaId = schemaId);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_bind", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Bind(this SettingsHandle settings, string key, ObjectHandle obj, string property, BindFlags flags);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_bind", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Bind(this SettingsHandle settings, string key, IntPtr tag, string property, BindFlags flags);

    public static bool SchemaHasKey(this SettingsHandle settings, string key)
    {
        var schema = GetDefaultSchema().GetSchema(settings.SchemaId, true);
        if (schema == 0)
            return false;
        var result = schema.SchemaHasKey(key);
        schema.Unref();
        return result;
    }

    public static SettingsHandle? ValidateKey(this SettingsHandle settings, string key)
        => settings.SchemaHasKey(key) ? settings : null;

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_set_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetBool(this SettingsHandle settings, string name, bool value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetBool(this SettingsHandle settings, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_set_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetInt(this SettingsHandle settings, string name, int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_get_int", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetInt(this SettingsHandle settings, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_create_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static ActionHandle CreateAction(this SettingsHandle settings, string key);

    public static string? GetString(this SettingsHandle settings, string key)
        => settings.ValidateKey(key)?._GetString(key).PtrToString(true);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetString(this SettingsHandle settings, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_has_key", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SchemaHasKey(this nint schema, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_source_get_default", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetDefaultSchema();

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_source_lookup", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetSchema(this nint schema, string id, bool recursive);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SettingsHandle _New(string schemaId);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void SchemaUnref(this nint schema);
}

