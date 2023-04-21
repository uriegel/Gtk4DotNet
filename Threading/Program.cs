using System;
using System.Threading;
using System.Threading.Tasks;
using GtkDotNet;

var app = Application.New("org.gtk.example");

Action onActivate = () => 
{
    Application.RegisterResources();
    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    var buttonBlocking = Builder.GetObject(builder, "buttonBlocking");
    var buttonBeginInvoke = Builder.GetObject(builder, "buttonBeginInvoke");
    var buttonEnableAsync = Builder.GetObject(builder, "buttonEnableAsync");
    var buttonAsync = Builder.GetObject(builder, "buttonAsync");

    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}");
    
    var label = Builder.GetObject(builder, "label");

    Gtk.SignalConnect(buttonEnableAsync, "clicked", Application.EnableSynchronizationContext);

    Gtk.SignalConnect(buttonBlocking, "clicked", () => 
    {
        Label.SetLabel(label, "");
        Thread.Sleep(10_000);
        Label.SetLabel(label, "Blocking finished");
    });

    Gtk.SignalConnect(buttonBeginInvoke, "clicked", () => 
    {
        Label.SetLabel(label, "");
        Task.Factory.StartNew(() =>
        {
            Console.WriteLine($"In Task, Thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(10_000);
            Application.BeginInvoke(100, () => 
            {
                Label.SetLabel(label, "BeginInvoke finished");
                Console.WriteLine($"In BeginInvoke, Thread {Thread.CurrentThread.ManagedThreadId}");
            });
        });
    });

    Gtk.SignalConnect(buttonAsync, "clicked", () => 
    {
        Run();

        async void Run()
        {
            Label.SetLabel(label, "");
            await Task.Delay(5000);
            Label.SetLabel(label, "Async finished");
            Console.WriteLine($"After async, Thread {Thread.CurrentThread.ManagedThreadId}");
        }
    });

    Window.SetApplication(window, app);
    GObject.Unref( builder);
    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

