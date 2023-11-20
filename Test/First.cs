using GtkDotNet;

static class First
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                        .SetTitle("Hello Gtk👍")
                        .SetDefaultSize(1200, 1200)
                        .Show())
            .Run(0, IntPtr.Zero);
}

