using System;
using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var window = Application.NewWindow(app);
    Window.SetTitle(window, "Hello Gtk👍");
    Window.SetDefaultSize(window, 1200, 1200);
    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;


