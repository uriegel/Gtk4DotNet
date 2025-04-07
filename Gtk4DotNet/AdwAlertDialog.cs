using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AdwAlertDialog
{
    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static AdwAlertDialogHandle New(string heading, string body);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static AdwAlertDialogHandle Heading(this AdwAlertDialogHandle dialog, string heading)
    {
        SetHeading(dialog, heading);
        return dialog;
    }
    public static AdwAlertDialogHandle Body(this AdwAlertDialogHandle dialog, string body)
    {
        SetBody(dialog, body);
        return dialog;
    }
    
    public static void OnResponse(this AdwAlertDialogHandle dialog, Action<string> onResponse)
        => Gtk.SignalConnect<AlertDialogResponseDelegate>(dialog, "response", (_, response, __) => onResponse(response));

    public static Task<string> PresentAsync(this AdwAlertDialogHandle dialog, WidgetHandle parent)
    {
        var tcs = new TaskCompletionSource<string>();
        dialog.OnResponse(tcs.SetResult);
        dialog.Present(parent);
        return tcs.Task;
    }

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_set_heading", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHeading(this AdwAlertDialogHandle dialog, string heading);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_set_body", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBody(this AdwAlertDialogHandle dialog, string body);
}
