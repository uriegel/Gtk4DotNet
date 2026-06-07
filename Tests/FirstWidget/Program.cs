using CsTools.Extensions;
using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics()
    .OnActivate(app => app
        .NewWindow()
        .Title("First Widget👍")
        .DefaultSize(200, 200)
        .Pipe(w => w.Child(
            Button
                .NewWithLabel("Maximize Window")
                .MarginStart(20)
                .MarginEnd(20)
                .MarginTop(20)
                .MarginBottom(20)
                .Clicked(() => w.IsMaximized = !w.IsMaximized)
                .Tooltip("This is a sample Button\tCtrl-H")))
        .Show()
    ).Run();

