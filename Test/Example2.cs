using GtkDotNet;

// TODO StackSwitcher Stack(RefHandle<StackHandle>) 
// TODO RefHandle if zero call callback when handle is set
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
                        .Titlebar(
                            HeaderBar
                            .New()
                            .TitleWidget(
                                StackSwitcher.New()
                            ))
                        .Child(
                            Box
                                .New(Orientation.Vertical)
                                .Append(
                                    Stack.New()))
                        .Show())
            .Run(0, IntPtr.Zero);
}

