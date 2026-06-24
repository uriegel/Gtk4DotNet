using CsTools.Extensions;
using Gtk4DotNet;

using static System.Console;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("Pack👍")
        .Pipe(win => win.Child(
            Grid
                .New()
                .Attach(                                
                    Button
                        .NewWithLabel("Button 1")
                        .Clicked(() => WriteLine("Button1 clicked")), 0, 0, 1, 1)
                .Attach(                                
                    Button
                        .NewWithLabel("Button 2")
                        .Clicked(() => WriteLine("Button2 clicked")), 1, 0, 1, 1)
                .Attach(                                
                    Button
                        .NewWithLabel("Quit")
                        .Clicked(() => win.CloseWindow()), 0, 1, 2, 1)))
        .Show()
    ).Run();
