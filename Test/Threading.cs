using GtkDotNet;
using LinqTools;
using static System.Console;

// TODO Application.EnableSynchronizationContext();
// TODO replace Threading Test with Hello World Scaffold and labels which are changed from Thread

static class Threading
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .SideEffect(_ => WriteLine($"Gkt theme: {GtkSettings.GetDefault().ThemeName}"))
                    .NewWindow()
                    .ResourceIcon("icon")
                    .Title("Hello Threading👍")
                    .DefaultSize(200, 200)
                    .OnClose(_ => false.SideEffect(_ => WriteLine("Window is closing")))
                    .SideEffect(w => w
                        .Child(
                            Box
                                .New(Orientation.Vertical)
                                .HAlign(Align.Center)
                                .VAlign(Align.Center)
                                .Append(
                                    Button
                                        .NewWithLabel("Maximize Window")
                                        .OnClicked(() => w.Maximize()))))
                    .Show())
            .Run(0, IntPtr.Zero);
}

