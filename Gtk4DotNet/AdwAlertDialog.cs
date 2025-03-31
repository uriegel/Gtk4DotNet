using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AdwAlertDialog
{
    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static AdwAlertDialogHandle New(string heading, string body);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_format_body", CallingConvention = CallingConvention.Cdecl)]
    public extern static void FormatBody(this AdwAlertDialogHandle dialog, string body);
}
