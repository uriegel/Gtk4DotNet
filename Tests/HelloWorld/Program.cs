using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app =>
    {
        using var window = app.NewWindow();
        window.Title("Hello World👍")
        .DefaultSize(600, 200)
        .Show();
    }
    ).Run();


