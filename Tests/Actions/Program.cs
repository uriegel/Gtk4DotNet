using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .Actions(new SimpleAction("test", () => Console.WriteLine("Test action from app"), "<Ctrl>T"))
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    ).Run();

