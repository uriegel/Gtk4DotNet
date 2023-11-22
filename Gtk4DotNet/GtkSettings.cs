using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GtkSettings
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_settings_get_default", CallingConvention = CallingConvention.Cdecl)]
    public extern static GtkSettingsHandle GetDefault();
} 

