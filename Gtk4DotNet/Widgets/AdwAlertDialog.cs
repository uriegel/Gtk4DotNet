using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class AdwAlertDialog : AdwDialog
{
    public static AdwAlertDialog New(string? heading = null, string? body = null)
    {
        var dialog = _New(heading, body);
        dialog.CheckDiagnostics();
        return dialog;
    }

    public void SetResponses(IEnumerable<AlertDialogResponse> responses, Action<string?> onResponse)
    {
        foreach (var response in responses.Reverse())
        {
            AddResponse(this, response.Id, response.Label);
            if (response.Default == true)
                SetDefaultResponse(this, response.Id);
            else if (response.Cancel == true)
                SetCloseResponse(this, response.Id);
            if (response.Appearance.HasValue)
                SetResponseAppearance(this, response.Id, response.Appearance.Value);
        }
        SignalConnect<ThreePointerDelegate>("response", (_, id, ___) => onResponse(id.PtrToString(false)));
    }

    public Task<string> PresentAsync(Widget parent)
    {
        var tcs = new TaskCompletionSource<string>();
        OnResponse(tcs.SetResult);
        Present(parent);
        return tcs.Task;
    }

    void OnResponse(Action<string> onResponse)
        => SignalConnect<AlertDialogResponseDelegate>("response", (_, response, __) => onResponse(response));

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    extern static AdwAlertDialog _New(string? heading, string? body);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_add_response", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddResponse(AdwAlertDialog dialog, string id, string label);


    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_set_default_response", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDefaultResponse(AdwAlertDialog dialog, string id);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_set_close_response", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetCloseResponse(AdwAlertDialog dialog, string id);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_alert_dialog_set_response_appearance", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetResponseAppearance(AdwAlertDialog dialog, string id, AdwResponseAppearance appearance);
}

public record AlertDialogResponse(
    string Id,
    string Label,
    bool? Default = null,
    bool? Cancel = null,
    AdwResponseAppearance? Appearance = null);