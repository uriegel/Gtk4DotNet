using GtkDotNet;

static class Example1
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                        .Title("Example Application 👍")
                        .DefaultSize(600, 400)
                        .Child(
                            Box
                                .New(Orientation.Vertical)
                                .Append(
                                    Stack.New()))
                        .Show())
            .Run(0, IntPtr.Zero);
}

