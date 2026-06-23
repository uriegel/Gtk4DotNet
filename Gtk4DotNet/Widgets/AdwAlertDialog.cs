using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// An Adwaita dialog presenting a message or a question. Alert dialogs have a heading, a body, an optional child widget, and one or multiple responses, each presented as a button.
/// Each response has a unique string ID, and a button label. Additionally, each response can be enabled or disabled, and can have a suggested or destructive appearance.
/// 
/// Response buttons can be presented horizontally or vertically depending on available space.
/// </summary>
public class AdwAlertDialog : AdwDialog
{
    /// <summary>
    /// Constructs a new <see cref="AdwAlertDialog"/> with a (optional) heading an a (optional) body.
    /// </summary>
    /// <param name="heading"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static AdwAlertDialog New(string? heading = null, string? body = null)
    {
        var dialog = _New(heading, body);
        dialog.CheckDiagnostics();
        return dialog;
    }

    /// <summary>
    /// Shows a dialog from a .NET resource template.ui and waits asynchronously until the dialog is closed. It returns on the UI thread and has to be called on thhe UI thread.
    /// </summary>
    /// <param name="template">The name of the .NET resource template.ui</param>
    /// <param name="name">The name of this dialog in the template.ui</param>
    /// <param name="parent">A window which is the parent of this dialog</param>
    /// <param name="ctor">Constructor to create an <see cref="AdwAlertDialog"/> or a custom dialog inherited from <see cref="AdwAlertDialog"/> with the help of a <see cref="Builder"/>.</param>
    /// <returns>When the dialog is closed, the response string of the response that closed the dailog</returns>
    public static Task<string> PresentFromTemplateAsync(string template, string name, Widget parent, Func<Builder, string, AdwAlertDialog>? ctor = null)
    {
        using var builder = Builder.FromDotNetResource(template);
        var dialog = ctor?.Invoke(builder, name) ?? new AdwAlertDialog(builder, name);
        return dialog.PresentAsync(parent);
    }

    /// <summary>
    /// Sets the responses to this dialog. Each response is represented by a button that can be configured by <see cref="AlertDialogResponse"/>.
    /// </summary>
    /// <param name="responses"></param>
    /// <param name="onResponse"></param>
    public void SetResponses(IEnumerable<AlertDialogResponse> responses, Action<string>? onResponse = null)
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
            SignalConnect<ThreePointerDelegate>("response", (_, id, ___) => onResponse(id.PtrToString(false) ?? ""));
    }

    /// <summary>
    /// Shows an AdwAlertDialog dialog and waits asynchronously until the dialog is closed. It returns on the UI thread and has to be called on thhe UI thread.
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
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