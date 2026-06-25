using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// Release ready

public class SettingsSchema : BaseHandle
{
    public GSettings NewSettings() => NewFull(this, 0, 0);
    protected override bool ReleaseHandle()
    {
        Unref(GetInternalHandle());
        return true;
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unref(nint settings);

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_new_full", CallingConvention = CallingConvention.Cdecl)]
    extern static GSettings NewFull(SettingsSchema schema, nint backend, nint path);
}