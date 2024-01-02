using GtkDotNet;
using CsTools.Extensions;
using static System.Console;

static class Css
{
    public static int Run()
        => Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app => 
                app
                    .SideEffect(_ => WriteLine($"Gkt theme: {GtkSettings.GetDefault().ThemeName}"))
                    .NewWindow()
                    .ResourceIcon("icon")
                    .SideEffect(w => StyleContext
                        .AddProviderForDisplay(Display.GetDefault(), 
                            CssProvider.New()
                                .FromResource("style"), StyleProviderPriority.Application))
                    .Title("Hello Gtk👍")
                    .DefaultSize(350, 100)
                    .OnClose(_ => false.SideEffect(_ => WriteLine("Window is closing")))
                    .SideEffect(w => w
                        .Child(
                            Box
                                .New(Orientation.Horizontal)
                                .HAlign(Align.Center)
                                .VAlign(Align.Center)
                                .Spacing(10)
                                .Append(
                                    Button
                                        .NewWithLabel("Maximize Window")
                                        .OnClicked(() => w.Maximize()))
                                .Append(
                                    Button
                                        .NewWithLabel("Is styled")
                                        .CssClass("styled"))))
                    .Show())
            .Run(0, IntPtr.Zero);
}

