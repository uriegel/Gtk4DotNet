using GtkDotNet;
using CsTools.Extensions;
using static System.Console;

static class NotDecorated
{
    public static int Run()
        => Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app => 
                app
                    .SideEffect(_ => WriteLine($"Gkt theme: {GtkSettings.GetDefault().ThemeName}"))
                    .NewWindow()
                    .ResourceIcon("icon")
                    .NotDecorated()
                    .Title("Hello Gtk👍")
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

