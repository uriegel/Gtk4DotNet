using Gtk4DotNet;


using var test = SettingsSchemaSource.GetDefault().Lookup(Globals.ApplicationId, true);
if (test == null)
    return;
test.Dispose();


Application
    .NewAdwaita(Globals.ApplicationId, ApplicationFlags.HandlesOpen)
    .WithDiagnostics(true)
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
