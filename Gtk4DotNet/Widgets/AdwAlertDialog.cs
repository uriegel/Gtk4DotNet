using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class AdwAlertDialog : AdwDialog
{
    // TODO New to construct a dialog
    // TODO Properties Heading and Body
    public Task<string> PresentAsync(Widget parent)
    {
        var tcs = new TaskCompletionSource<string>();
        OnResponse(tcs.SetResult);
        Present(parent);
        return tcs.Task;
    }

    void OnResponse(Action<string> onResponse)
        => SignalConnect<AlertDialogResponseDelegate>("response", (_, response, __) => onResponse(response));
}
