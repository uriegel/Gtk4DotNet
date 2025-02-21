using GtkDotNet;

static class First
{
    public static int Run()
        => Application
                .New("org.gtk.example")
                .OnActivate(app =>
                    app
                        .NewWindow()
                            .Title("Hello Gtk👍")
                            .DefaultSize(600, 200)
                            .Show())
                .Run(0, IntPtr.Zero);
}
