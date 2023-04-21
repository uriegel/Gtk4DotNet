using System;
using GtkDotNet;

void clicked() => Console.WriteLine("Clicked button");

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var window = Application.NewWindow(app);
    Window.SetTitle(window, "Hello Gtk👍");
    
    var grid = Grid.New();
    Window.SetChild(window, grid);

    var button = Button.NewWithLabel("Button 1");
    Gtk.SignalConnect(button, "clicked", clicked);
    Grid.Attach (grid, button, 0, 0, 1, 1);

    button = Button.NewWithLabel("Button 2");
    Gtk.SignalConnect(button, "clicked", clicked);
    Grid.Attach(grid, button, 1, 0, 1, 1);

    button = Button.NewWithLabel("Quit");
    Gtk.SignalConnect(button, "clicked", () => Window.Close(window));
    Grid.Attach(grid, button, 0, 1, 2, 1);

    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;


