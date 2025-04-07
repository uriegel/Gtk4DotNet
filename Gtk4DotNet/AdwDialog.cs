using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AdwDialog
{
    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static AdwDialogHandle New();

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_present", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Present(this AdwDialogHandle dialog, WidgetHandle parent);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_set_presentation_mode", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetPresentationMode(this AdwDialogHandle dialog, DialogPresentationMode mode);
}