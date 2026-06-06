using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .OnActivate(app => app
        .NewWindow()
        .Title("Hello First👍")
        .DefaultSize(600, 200)
        .Show()
    ).Run();

