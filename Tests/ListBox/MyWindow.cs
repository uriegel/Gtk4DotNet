using System.Diagnostics;
using System.Runtime.InteropServices;
using Gtk4DotNet;

// TODO Group items (like in Nautilus)
// TODO Dictionary map nint to ListItem, onFinalize remove item from Dictionary
class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        var stopuhr = new Stopwatch();
        stopuhr.Start();
        using var appinfos = GAppInfo.GetAllApps();
        listbox.SetHeaderFunc((r, rp) =>
        {
            Console.WriteLine($"{r}, {rp}");
            var item = GetListItem(r);
            if (rp == 0)
                SetHeader(r, Label.New("Das ist der tolle Hedder"));
        });
        foreach (var appinfo in appinfos.OrderBy(n => n.Name).Where(n => n.ShouldShow))
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, appinfo.GetIcon(), appinfo.Name));
        var ela = stopuhr.Elapsed;
        Console.WriteLine(ela);
    }

    [Widget]
    readonly ListBox listbox = null!;

    [DllImport("libgtk-4.so.1", EntryPoint = "gtk_list_box_row_get_child", CallingConvention = CallingConvention.Cdecl)]
    extern static ListItem GetListItem(nint row);

    [DllImport("libgtk-4.so.1", EntryPoint = "gtk_list_box_row_set_header", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHeader(nint row, Widget header);
    

}

