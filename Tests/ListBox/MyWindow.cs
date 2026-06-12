using Gtk4DotNet;

// TODO BuildFromTemplate: all controls are not freed
// TODO memoize Builder
// TODO measure creation time
// TODO button to clear ListBox (are itels finalized?)
// TODO only the applictions with a reasonable icon (like in Nautilus)
// TODO Group items (like in Nautilus)

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        using var appinfos = GAppInfo
            .GetAllApps();
        foreach (var appinfo in appinfos.OrderBy(n => n.Name).Where(n => n.ShouldShow))
        {
            using var listitem = ListItem.New(appinfo.GetIcon(), appinfo.Name);
            // var listitem = Box.New(Orientation.Horizontal)
            //     .Append(Image.NewFromIcon(appinfo.GetIcon()))
            //     .Append(Label.New(appinfo.Name ?? ""));
            listbox.Append(listitem);
        }
    }

    [Widget]
    readonly ListBox listbox = null!;
}

