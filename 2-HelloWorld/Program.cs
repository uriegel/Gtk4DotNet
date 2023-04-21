using System;
using GtkDotNet;

void clicked() => Console.WriteLine("Clicked button");

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var window = Application.NewWindow(app);
    Window.SetTitle(window, "Hello Gtk👍");
    Window.SetDefaultSize(window, 200, 200);

    var box = Box.New(GtkDotNet.Orientation.Vertical, 0);
    Widget.SetHAlign(box, GtkDotNet.Align.Center);
    Widget.SetVAlign(box, GtkDotNet.Align.Center);

    Window.SetChild(window, box);

    var button = Button.NewWithLabel("Hello Wörld");
    Gtk.SignalConnect(button, "clicked", clicked);

    Box.Append(box, button);

    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;


