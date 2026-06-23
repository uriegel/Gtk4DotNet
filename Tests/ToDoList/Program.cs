using Gtk4DotNet;

Application
    .NewAdwaita("org.gtkrs.Todo")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    ).Run();
