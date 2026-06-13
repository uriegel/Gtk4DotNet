using Gtk4DotNet;

// TODO set description from file name
// TODO Cancel Button 
// TODO default action "Open File"
// TODO AppChooserWidget in an AdwDialog
// TODO open file

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        dialogFromCode.OnClicked(OnDialog);
        dialogFromResource.OnClicked(OnDialogFromResource);
        dialogAppChooser.OnClicked(OnAppChoser);
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
        => await AdwAlertDialog.PresentFromTemplateAsync("dialog", "dialog", this);

    void OnAppChoser()
        => AdwDialog.PresentFromTemplate("appchooser", "dialog", this, (builder, name) => new AppChooser(builder, name));

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

    [Widget]
    readonly Button dialogAppChooser = null!;
}
