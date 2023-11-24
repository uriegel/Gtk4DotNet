using GtkDotNet;
using GtkDotNet.SafeHandles;
using LinqTools;
using static System.Console;

static class Cleanup
{
    public static int Run()
    {
        Test(Check1, "Finished 1");
        Test(Check2, "Finished 2");
        Test(Check3, "Finished 3");
        Test(Check4, "Finished 4");
        Test(Check5, "Finished 5");
        Test(Check5, "Finished 5");
        Test(Check5, "Finished 5");
        Test(Check5, "Finished 5");
        Test(Check6, "Finished 6");
        Test(Check7, "Finished 7");
        return 0;
    }

    static void Check1()
    {
        var test1 = new Test1();
    }

    static void Check2()
        => Application
            .New("de.urigel.test")
            .OnActivate(app =>
            {
                var test1 = new Test1();
                var button = Button.NewWithLabel("Hello");
                button.AddWeakRef(() => WriteLine("Button Check2 disposed"));
                // (dotnet:24121): Gtk-WARNING **: 13:12:24.890: A floating object was finalized. This means that someone
                // called g_object_unref() on an object that had only a floating
                // reference; the initial floating reference is not owned by anyone
                // and must be removed with g_object_ref_sink().
                button.RefSink();
            })
            .Run(0, IntPtr.Zero);

    static void Check3()
        => Application
            .New("de.urigel.test")
            .AddWeakRef(() => WriteLine("Application Check3 disposed"))
            .AddWeakRef(() => WriteLine("Application Check3 disposed"))
            .AddWeakRef(() => WriteLine("Application Check3 disposed"))
            .OnActivate(app => 
            {
                var test1 = new Test1();
            })
            .Run(0, IntPtr.Zero);

    static void Check4()
        => Application
            .New("de.urigel.test")
            .OnActivate(app => 
            {
                var test1 = new Test1();
                app
                    .NewWindow()
                    .AddWeakRef(() => WriteLine("Window Check4 disposed"))
                    .Title("Hello Gtk👍")
                    .Show();
            })
            .Run(0, IntPtr.Zero);

    static void Check5()
        => Application
            .New("de.urigel.test")
            .AddWeakRef(() => WriteLine("Application Check5 disposed"))
            .OnActivate(app =>
            {
                var test1 = new Test1();
                app
                    .NewWindow()
                    .AddWeakRef(() => WriteLine("Window Check5 disposed"))
                    .OnClose(_ => false.SideEffect(_ =>
                    {
                        var test1 = new Test1();
                        WriteLine("Window5 is closing");
                    }))
                    .Title("Hello Gtk👍")
                    .Show();
            })
            .Run(0, IntPtr.Zero);

    static void Check6()
        => Application
            .New("de.urigel.test")
            .AddWeakRef(() => WriteLine("Application Check6 disposed"))
            .OnActivate(app => 
            {
                var test1 = new Test1();
                app
                    .NewWindow()
                    .AddWeakRef(() => WriteLine("Window Check6 disposed"))
                    .Title("Hello Gtk👍")
                    .Child(Button
                        .NewWithLabel("Button")
                        .OnClicked(() => WriteLine("clicked 6"))
                        .AddWeakRef(() => WriteLine("Button Check6 disposed")))
                    .Show();
            })
            .Run(0, IntPtr.Zero);

    static void Check7()
        => Application
            .New("de.urigel.test")
            .AddWeakRef(() => WriteLine("Application Check7 disposed"))
            .OnActivate(app => 
            {
                var test1 = new Test1();
                app
                    .NewWindow()
                    .Ref(window)
                    .AddWeakRef(() => WriteLine("Window Check7 disposed"))
                    .Title("Hello Gtk👍")
                    .Child(Button
                        .NewWithLabel("Button")
                        .OnClicked(OnDialog)
                        .AddWeakRef(() => WriteLine("Button Check7 disposed")))
                    .Show();
            })
            .Run(0, IntPtr.Zero);

    static void OnDialog()
    {
        Dialog
            .New("Hello World beenden?", window.Ref, DialogFlags.DestroyWithParent | DialogFlags.Modal, "Ok", Dialog.RESPONSE_OK)
            .AddWeakRef(() => WriteLine("Dialog Check6 disposed"))
            .Show();
    }

    static void Test(Action action, string text)
    {
        action();
        GC.Collect();
        GC.Collect();
        Thread.Sleep(1000);
        GC.Collect();
        GC.Collect();
        WriteLine(text);
        WriteLine($"Signal handlers: {GtkDelegates.Instances}");
        ReadLine();
    }

    static WidgetRef<WindowHandle> window = new();
}

class Test1
{
   string test = "Hallo";

    ~Test1()
        => WriteLine("Test1 disposed");
}

