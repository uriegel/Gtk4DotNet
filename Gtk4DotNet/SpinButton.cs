using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SpinButton
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_spin_button_get_value", CallingConvention = CallingConvention.Cdecl)]
    public extern static double GetValue(this SpinButtonHandle spinButton);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_spin_button_set_value", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetValue(this SpinButtonHandle spinButton, double value);
}
