//#define Program
#if Program

using System;
using GtkDotNet;

Console.WriteLine("Hello Gtk 4");

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var window = Application.NewWindow(app);
    Window.SetTitle(window, "Listbox 👍");
    Window.SetDefaultSize(window, 600, 300);

    var listbox = Listbox.New();
    for (var i = 0; i < 10_000; i++)
    {
        var label = Label.New($"Numero {i}");
        Listbox.Prepend(listbox, label);
    }

    var scrolledWindow = ScrolledWindow.New();
    ScrolledWindow.SetPolicy(scrolledWindow, GtkDotNet.PolicyType.Never, GtkDotNet.PolicyType.Automatic);
    ScrolledWindow.SetMinContentWidth(scrolledWindow, 360);
    ScrolledWindow.SetChild(scrolledWindow, listbox);
    Window.SetChild(window, scrolledWindow);

    Widget.Show(window);
};


return Application.Run(app, onActivate);

#endif