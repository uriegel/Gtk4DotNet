using GtkDotNet;

static class Example2
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
                        .Titlebar(
                            HeaderBar
                            .New()
                            .TitleWidget(
                                StackSwitcher.New()
                            ))
                        .Show())
            .Run(0, IntPtr.Zero);
}

