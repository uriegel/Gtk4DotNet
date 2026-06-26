using CsTools.Extensions;
using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("Multiple Window👍")
        .Child(Button
            .NewWithLabel("Create Window")
            .SideEffect(b => b.OnClicked += () =>
            {
                var win = new MyWindow(app);
                win.Show();
            }))
        .Show()
    ).Run();