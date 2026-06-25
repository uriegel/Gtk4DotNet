using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.exampleapp", ApplicationFlags.HandlesOpen)
    .WithDiagnostics(true)
    .WithSettings()
    .OnOpen((app, files) =>
    {
        if (MyWindow.Instance == null)
            app
                .WindowFromBuilder("window", "window", p => new MyWindow(p))
                .Show();
        foreach (var file in files)
            MyWindow.Instance?.OnOpen(file);
    })
    .OnActivate(app => app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    ).Run();
