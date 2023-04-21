using System;
using GtkDotNet;

void clicked() => Console.WriteLine("Clicked button");

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    Application.RegisterResources();
    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    var button1 = Builder.GetObject(builder, "button1");
    var button2 = Builder.GetObject(builder, "button2");
    var quit = Builder.GetObject(builder, "quit");
    Window.SetApplication(window, app);
    GObject.Unref( builder);

    Gtk.SignalConnect(button1, "clicked", clicked);
    Gtk.SignalConnect(button2, "clicked", clicked);
    Gtk.SignalConnect(quit, "clicked", () => Window.Close(window));

    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

