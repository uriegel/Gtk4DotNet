using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// Release ready

public class SettingsSchema : BaseHandle
{
    protected override bool ReleaseHandle()
    {
        Unref(GetInternalHandle());
        return true;
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "g_settings_schema_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unref(nint settings);
}