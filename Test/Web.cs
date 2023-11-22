using GtkDotNet;
using LinqTools;

using static System.Console;

static class Web
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Hello Web View👍")
                    .DefaultSize(800, 600)
                    .Child(
                        WebView
                            .New()
                            .SideEffect(w => w.GetSettings()
                                .SideEffect(s => 
                                {
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                    WriteLine($"CursiveFontFamily: {s.CursiveFontFamily}");
                                    s.EnableDeveloperExtras = true;
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                }))
                            .LoadUri($"file://{Directory.GetCurrentDirectory()}/webroot/index.html")
                    )
                    .Show())
            .Run(0, IntPtr.Zero);
}


