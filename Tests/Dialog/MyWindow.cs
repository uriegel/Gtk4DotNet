using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        dialogFromCode.OnClicked(OnDialog);
        dialogFromResource.OnClicked(OnDialogFromResource);
        OnClose(PreventClosing);
    }

    async void OnDialog()
    {
        var dialog = AdwAlertDialog.New("Save changes?", "Do you want to save your changes?");
        dialog.SetResponses([
                new("yes", "_Yes", Default: true, Appearance: AdwResponseAppearance.Suggested),
                new("no", "_No", Appearance: AdwResponseAppearance.Destructive),
                new("cancel", "_Cancel", Cancel: true)
            ], Console.WriteLine);
        var res = await dialog.PresentAsync(this);
    }

    async void OnDialogFromResource()
    {
        using var bilder = Builder.FromDotNetResource("dialog");
        var dialog = bilder.GetWidget<AdwAlertDialog>("dialog");
        await dialog.PresentAsync(this);
    }

    bool PreventClosing(Window window)
    {
        var dialog = AdwAlertDialog.New("Close Window?", "Do you want to close the application?");
        dialog.SetResponses([
                new("ok", "_Ok", Default: true),
                new("cancel", "_Cancel", Cancel: true)
            ], Console.WriteLine);
        PresentAsync();
        return true;

        async void PresentAsync()
        {
            var result = await dialog.PresentAsync(window);
            Console.WriteLine($"Dialog result: {result}");
        }
    }

    [Widget]
    readonly Button dialogFromCode = null!;

    [Widget]
    readonly Button dialogFromResource = null!;
}


// TODO Dialog asking close or not? force closing
