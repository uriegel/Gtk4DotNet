using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class ToggleButton
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ToggleButtonHandle New();
}


 