using System.Diagnostics;
using Gtk4DotNet;

// TODO Group items (like in Nautilus)

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        var stopuhr = new Stopwatch();
        stopuhr.Start();
        using var appinfos = GAppInfo.GetAllApps();
        foreach (var appinfo in appinfos.OrderBy(n => n.Name))//.Where(n => n.ShouldShow))
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, appinfo.GetIcon(), appinfo.Name));
        var ela = stopuhr.Elapsed;
        Console.WriteLine(ela);
    }

    [Widget]
    readonly ListBox listbox = null!;
}

