using System.Diagnostics;
using Gtk4DotNet;

// TODO Group items (like in Nautilus)
// TODO ListBoxHeaderWidget
// TODO SetHeader
// TODO Test with Finalizer in ListItem
class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        listbox.SetHeaderFunc<ListItem>((current, previous) =>
        {
            Console.WriteLine($"{current?.Name}, {previous?.Name}");


            //  if (previous == null)
            //     SetHeader(r, Label.New("Das ist der tolle Hedder"));
            // [DllImport("libgtk-4.so.1", EntryPoint = "gtk_list_box_row_set_header", CallingConvention = CallingConvention.Cdecl)]
            // extern static void SetHeader(nint row, Widget header);
        });

        var stopuhr = new Stopwatch();
        stopuhr.Start();
        using var appinfos = GAppInfo.GetAllApps();
        foreach (var appinfo in appinfos.OrderBy(n => n.Name).Where(n => n.ShouldShow))
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, appinfo.GetIcon(), appinfo.Name).RegisterWidget());
        var ela = stopuhr.Elapsed;
        Console.WriteLine(ela);
    }

    [Widget]
    readonly ListBox listbox = null!;


}

