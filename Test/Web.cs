using GtkDotNet;

static class Web
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Web View👍")
                    .Show())
            .Run(0, IntPtr.Zero);
}
