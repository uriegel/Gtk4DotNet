using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        dialog1.OnClicked(OnDialog1);
    }

    void OnDialog1()
    {
        var dialog = AdwAlertDialog.New("Save changes?", "Do you want to save your changes?");
        dialog.SetResponses([
                new("yes", "_Yes", Default: true, Appearance: AdwResponseAppearance.Suggested),
                new("no", "_No", Appearance: AdwResponseAppearance.Destructive),
                new("cancel", "_Cancel", Cancel: true)
            ], Console.WriteLine);
        dialog.PresentAsync(this);
    }

    [Widget]
    readonly Button dialog1 = null!;
}

// TODO Button 2 Dialog from builder, Cambalache


// static bool PreventClosing(ApplicationWindow window)
// {
//     using var bilder = Builder.FromDotNetResource("dialog");
//     var dialog = bilder.GetWidget<AdwAlertDialog>("dialog");
//     PresentAsync();
//     return true;

//     async void PresentAsync()
//     {
//         var result = await dialog.PresentAsync(window);
//         Console.WriteLine($"Dialog result: {result}");
//     }
// }

// TODO Dialog asking close or not? force closing
