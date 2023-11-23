using System.Runtime.InteropServices;
using GtkDotNet;
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
            .OnActivate(app => 
            {
                var test1 = new Test1();
            })
            .Run(0, IntPtr.Zero);

    static void Check4()
        => Application
            .New("de.urigel.test")
            .AddWeakRef(() => WriteLine("Application Check4 disposed"))
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
                    .Title("Hello Gtk👍")
                    .Child(Button
                        .NewWithLabel("Button")
                        .AddWeakRef(() => WriteLine("Button Check5 disposed")))
                    .Show();
            })
            .Run(0, IntPtr.Zero);

    static void Test(Action action, string text)
    {
        action();
        GC.Collect();
        GC.Collect();
        Thread.Sleep(1000);
        GC.Collect();
        GC.Collect();
        WriteLine(text);
        ReadLine();
    }
}

class Test1
{
   string test = "Hallo";

    ~Test1()
        => WriteLine("Test1 disposed");
}

