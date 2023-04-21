using System;
using System.Diagnostics;
using System.Linq;
using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    Application.RegisterResources();
    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    var stack = Builder.GetObject(builder, "stack");
    var gears = Builder.GetObject(builder, "gears");
    Window.SetApplication(window, app);
    GObject.Unref(builder);

    var menuBuilder = Builder.FromResource("/org/gtk/example/menu.ui");
    var menu = Builder.GetObject(menuBuilder, "menu");
    MenuButton.SetModel(gears, menu);
    GObject.Unref(menuBuilder);

    var actions = new GtkAction[] 
    {
        new GtkAction("preferences", () => Console.WriteLine("Preferenz")),
        new GtkAction("quit", () => Window.Close(window), "<Ctrl>Q")
    };
    Application.AddActions(app, actions);

    Widget.Show(window);

    var currentDirectory = Directory.GetCurrentDirectory();
    var files = System.Environment.CommandLine.Split(' ').Skip(1).Select(n => GFile.New(Path.Combine(currentDirectory, n)));
    foreach (var file in files)
    {
        var name = GFile.GetBasename(file);
        var scrolled = ScrolledWindow.New ();
        Widget.SetHExpand(scrolled, true);
        Widget.SetVExpand(scrolled, true);
        var textView = TextView.New();
        TextView.SetEditable(textView, false);
        TextView.SetCursorVisible(textView, false);
        ScrolledWindow.SetChild(scrolled, textView);
        Stack.AddTitled(stack, scrolled, name, name);
        var content = GFile.LoadContents(file);
        if (content.HasValue)
        {
            var buffer = TextView.GetBuffer(textView);
            TextBuffer.SetText(buffer, content.Value.content, (int)content.Value.length);
            GObject.Free(content.Value.content);
        }
    }
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

