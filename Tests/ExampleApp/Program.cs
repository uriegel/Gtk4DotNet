using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet", ApplicationFlags.HandlesOpen)
    .WithDiagnostics(true)
    .OnOpen((app, files) =>
    {
        if (MyWindow.Instance == null)
            app
                .WindowFromBuilder("window", "window", p => new MyWindow(p))
                .Show();
        var feilen = files;
    })
    .OnActivate(app => app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    ).Run();
