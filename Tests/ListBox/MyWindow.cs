using Gtk4DotNet;

// TODO memoize Builder
// TODO measure creation time
// TODO GIcon as SafeHandle
// TODO only the applictions with a reasonable icon (like in Nautilus)
// TODO Group items (like in Nautilus)

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        using var appinfos = GAppInfo.GetAllApps();
        foreach (var appinfo in appinfos.OrderBy(n => n.Name))
        {
            var listitem = ListItem.New(appinfo.GetGIcon(), appinfo.Name);
            listbox.Append(listitem);
        }
    }

    [Widget]
    readonly ListBox listbox = null!;
}

