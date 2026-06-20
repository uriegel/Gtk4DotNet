using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// TODO Release ready

public class AdwAlertDialog : AdwDialog
{
    public static AdwAlertDialog New(string? heading = null, string? body = null)
    {
        var dialog = _New(heading, body);
        dialog.CheckDiagnostics();
        return dialog;
    }

    public static Task<string> PresentFromTemplateAsync(string template, string name, Widget parent, Func<Builder, string, AdwAlertDialog>? ctor = null)
    {
        using var builder = Builder.FromDotNetResource(template);
        var dialog = ctor?.Invoke(builder, name) ?? new AdwAlertDialog(builder, name);
        return dialog.PresentAsync(parent);
    }

    public void SetResponses(IEnumerable<AlertDialogResponse> responses, Action<string?>? onResponse = null)
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
        if (onResponse != null)
            SignalConnect<ThreePointerDelegate>("response", (_, id, ___) => onResponse(id.PtrToString(false)));
    }

    public Task<string> PresentAsync(Widget parent)
    {
        var tcs = new TaskCompletionSource<string>();
        OnResponse(tcs.SetResult);
        Present(parent);
        return tcs.Task;
    }

    protected AdwAlertDialog(Builder builder, string? name = null) : base(builder, name) { }

    AdwAlertDialog() : base() { }

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