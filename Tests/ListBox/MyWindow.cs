using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        using var appinfos = GAppInfo.GetAllApps();
        foreach (var appinfo in appinfos.OrderBy(n => n.Name))
        {
            listbox.Append(Box
                .New(Orientation.Horizontal, 5)
                .Margin(5)
                .Append(Image.NewFromGIcon(appinfo.GetGIcon()))
                .Append(Label.New(appinfo.Name ?? "-"))
            );    
        }
    }

    [Widget]
    readonly ListBox listbox = null!;
}

