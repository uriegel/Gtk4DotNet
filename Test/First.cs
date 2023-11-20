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
                        .DefaultSize(1200, 1200)
                        .Show())
            .Run(0, IntPtr.Zero);
}

