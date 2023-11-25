using GtkDotNet;
using GtkDotNet.SafeHandles;
using LinqTools;
using static System.Console;

static class Threading
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .SideEffect(_ => WriteLine($"Gkt theme: {GtkSettings.GetDefault().ThemeName}"))
                    .NewWindow()
                    .ResourceIcon("icon")
                    .Title("Hello Threading👍")
                    .DefaultSize(200, 200)
                    .OnClose(_ => false.SideEffect(_ => WriteLine("Window is closing")))
                    .SideEffect(w => w
                        .Child(
                            Box
                                .New(Orientation.Vertical)
                                .HAlign(Align.Center)
                                .VAlign(Align.Center)
                                .Append(
                                    Label
                                        .New("Choose...")
                                        .Ref(label))
                                .Append(
                                    Button
                                        .NewWithLabel("Invoke synchronously")
                                        .OnClicked(LongTime))
                                .Append(
                                    Button
                                        .NewWithLabel("Begin invoke")
                                        .OnClicked(BeginInvoke))
                                .Append(
                                    Button
                                        .NewWithLabel("Invoke asynchronously")
                                        .OnClicked(AsyncInvoke))))
                    .Show())
            .Run(0, IntPtr.Zero);

    static void LongTime()
    {
        label.Ref.Set("before 'long time'");
        Thread.Sleep(10_000);
        label.Ref.Set("after 'long time'");
    }

    static void BeginInvoke()
    {
        label.Ref.Set("before 'BeginInvoke'");
        Task.Factory.StartNew(() =>
        {
            WriteLine($"In Task, Thread {Environment.CurrentManagedThreadId}");
            Thread.Sleep(10_000);
            Gtk.BeginInvoke(100, () => 
            {
                label.Ref.Set("after 'BeginInvoke'");
                WriteLine($"In BeginInvoke, Thread {Environment.CurrentManagedThreadId}");
            });
        });
    }

    static async void AsyncInvoke()
    {
        WriteLine($"Entering AsyncInvoke, Thread {Environment.CurrentManagedThreadId}");
        label.Ref.Set("before 'AsyncInvoke'");
        await Task.Delay(10_000);
        label.Ref.Set("after 'AsyncInvoke'");
        WriteLine($"Leaving AsyncInvoke, Thread {Environment.CurrentManagedThreadId}");
    }

    static readonly ObjectRef<LabelHandle> label = new();
}

