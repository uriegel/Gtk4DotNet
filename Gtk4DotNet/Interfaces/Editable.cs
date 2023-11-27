using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.Interfaces;

public static class Editable 
{
    public static string? GetText(WidgetHandle handle)
        => _GetText(handle).PtrToString(false);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetText(WidgetHandle editable, string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetText(WidgetHandle editable);

}
