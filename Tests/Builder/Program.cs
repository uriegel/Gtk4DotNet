using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .OnActivate(app => app
        .WindowFromBuilder("template", "window", p => new MyWindow(p))
        .Show()
    ).Run();

