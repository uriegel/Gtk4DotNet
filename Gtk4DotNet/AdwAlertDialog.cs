using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AdwAlertDialog
{
    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static AdwAlertDialogHandle New(string heading, string body);

    public static void OnResponse(this AdwAlertDialogHandle dialog, Action<string> onResponse)
        => Gtk.SignalConnect<AlertDialogResponseDelegate>(dialog, "response", (_, response, __) => onResponse(response));

    public static Task<string> PresentAsync(this AdwAlertDialogHandle dialog, WidgetHandle parent)
    {
        var tcs = new TaskCompletionSource<string>();
        dialog.OnResponse(tcs.SetResult);
        dialog.Present(parent);
        return tcs.Task;
    }
}
