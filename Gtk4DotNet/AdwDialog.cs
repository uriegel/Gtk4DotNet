using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AdwDialog
{
    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static AdwDialogHandle New();

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_present", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Present(this AdwDialogHandle dialog, WidgetHandle parent);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_set_presentation_mode", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetPresentationMode(this AdwDialogHandle dialog, DialogPresentationMode mode);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_set_default_widget", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetDefaultWidget(this AdwDialogHandle dialog, WidgetHandle defaultWidget);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_close", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool CloseDialog(this AdwDialogHandle dialog);

    public static void OnClosed(this AdwDialogHandle dialog, Action onClosed)
        => Gtk.SignalConnect<TwoPointerDelegate>(dialog, "closed", (_, __) => onClosed());

}