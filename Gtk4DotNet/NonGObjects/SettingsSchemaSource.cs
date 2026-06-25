using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// Release ready

public class SettingsSchemaSource : BaseHandle
{
    public static SettingsSchemaSource FromDirectory(string directory, SettingsSchemaSource? parent = null, bool truested = true)
    {
        nint error = 0;
        var source = FromDirectory(directory, parent != null ? parent.GetInternalHandle() : 0, truested, ref error);
        if (source.IsInvalid)
            throw GtkException.Get(error, true);
        source.unref = true;
        return source;
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_source_get_default", CallingConvention = CallingConvention.Cdecl)]
    public extern static SettingsSchemaSource GetDefault();

    public SettingsSchema? Lookup(string schemaId, bool recursive)
    {
        var res = Lookup(this, schemaId, recursive);
        return !res.IsInvalid ? res : null;
    }

    protected override bool ReleaseHandle()
    {
        if (unref)
            Unref(GetInternalHandle());
        return true;
    }

    bool unref;

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_source_lookup", CallingConvention = CallingConvention.Cdecl)]
    extern static SettingsSchema Lookup(SettingsSchemaSource source, string schemaId, bool recursive);

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_source_new_from_directory", CallingConvention = CallingConvention.Cdecl)]
    extern static SettingsSchemaSource FromDirectory(string directory, nint parent, bool truested, ref nint error);

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_source_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static SettingsSchemaSource Unref(nint handle);
}
