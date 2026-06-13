using Gtk4DotNet;

// TODO AdwDialog with Cancel "Open file" Open, Box with description and ...
// TODO AppChooserWidget in an AdwDialog

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        dialogFromCode.OnClicked(OnDialog);
        dialogFromResource.OnClicked(OnDialogFromResource);
        OnCloseAsync(PreventClosing);
    }

    async void OnDialog()
    {
        var dialog = AdwAlertDialog.New("Save changes?", "Do you want to save your changes?");
        dialog.SetResponses([
                new("yes", "_Yes", Default: true, Appearance: AdwResponseAppearance.Suggested),
                new("no", "_No", Appearance: AdwResponseAppearance.Destructive),
                new("cancel", "_Cancel", Cancel: true)
            ]);
        var res = await dialog.PresentAsync(this);
    }

    async void OnDialogFromResource()
    {
        using var bilder = Builder.FromDotNetResource("dialog");
        var dialog = bilder.GetWidget<AdwAlertDialog>("dialog");
        await dialog.PresentAsync(this);
    }

    async Task<bool> PreventClosing(Window window)
    {
        var dialog = AdwAlertDialog.New("Close Window?", "Do you want to close the application?");
        dialog.SetResponses([
                new("ok", "_Ok", Default: true),
                new("cancel", "_Cancel", Cancel: true)
            ]);
        return await dialog.PresentAsync(window) != "ok";
    }

    [Widget]
    readonly Button dialogFromCode = null!;

    [Widget]
    readonly Button dialogFromResource = null!;
}
