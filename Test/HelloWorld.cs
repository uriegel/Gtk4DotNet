using GtkDotNet;

using static System.Console;

static class HelloWorld
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                    .Title("Hello Gtk👍")
                    .DefaultSize(200, 200)
                    .Child(
                        Box
                            .New(Orientation.Vertical)
                            .HAlign(Align.Center)
                            .VAlign(Align.Center)
                            .Append(
                                Button
                                     .NewWithLabel("Hello Wörld")
                                     .OnClicked(() => WriteLine("Button clicked"))))
                    .Show())
            .Run(0, IntPtr.Zero);
}

