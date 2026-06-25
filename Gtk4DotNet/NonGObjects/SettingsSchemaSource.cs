using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// Release ready

public class SettingsSchemaSource : BaseHandle
{
    public SettingsSchema? Lookup(string schemaId, bool recursive)
    {
        var res = Lookup(this, schemaId, recursive);
        return !res.IsInvalid ? res : null;
    }
    
    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_source_get_default", CallingConvention = CallingConvention.Cdecl)]
    public extern static SettingsSchemaSource GetDefault();

    protected override bool ReleaseHandle() => true;

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_source_lookup", CallingConvention = CallingConvention.Cdecl)]
    extern static SettingsSchema Lookup(SettingsSchemaSource source, string schemaId, bool recursive);
}
