using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics()
    .OnActivate(app => app
        .NewWindow()
        .Title("Hello Dialog👍")
        .DefaultSize(600, 200)
        .Closing(PreventClosing)
        .Show()
    ).Run();


static bool PreventClosing(ApplicationWindow window)
{
    using var bilder = Builder.FromDotNetResource("dialog");
    var dialog = bilder.GetWidget<AdwAlertDialog>("dialog");
    PresentAsync();
    return true;

    async void PresentAsync()
    {
        var result = await dialog.PresentAsync(window);
        Console.WriteLine($"Dialog result: {result}");
    }
}

// TODO Dialog asking close or not? force closing
