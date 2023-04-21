using System;
using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    Application.RegisterResources();
    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    Window.SetApplication(window, app);
    GObject.Unref( builder);
    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

