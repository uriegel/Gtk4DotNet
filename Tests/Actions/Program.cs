using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics()
    .OnActivate(app => app
        .NewWindow()
        .Title("Hello World👍")
        .DefaultSize(600, 200)
        .Show()
    ).Run();


