using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class DropDown
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drop_down_get_selected", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetSelected(this DropDownHandle dropDown);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drop_down_set_selected", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSelected(this DropDownHandle dropDown, int pos);
}
