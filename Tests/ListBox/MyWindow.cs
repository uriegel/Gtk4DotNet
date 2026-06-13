using System.Diagnostics;
using Gtk4DotNet;

// TODO Header from template like Nautilus
// TODO Test with Finalizer in ListItem
class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        listbox.SetHeaderFunc<ListItem>((current, previous) =>
        {
            if (previous == null && current != null)
                current.SetHeader(Label.New("Recommended Apps"));
            var currentListitem = current?.GetChild<ListItem>();
            var previousListitem = previous?.GetChild<ListItem>();
            if (previousListitem?.IsRecommended == true && currentListitem?.IsRecommended == false)
                current?.SetHeader(Label.New("All Apps"));
        });

        var stopuhr = new Stopwatch();
        stopuhr.Start();
        var contentType = Gio.GuessContentType(".html") ?? "none";
        using var defaultApp = GAppInfo.GetDefault(contentType);
        listbox.AppendFromTemplate("listitem", b => new ListItem(b, defaultApp.GetIcon(), defaultApp.Name, true).RegisterWidget());
        using var recommendedApps = GAppInfo.GetRecommendedApps(contentType);
        foreach (var appinfo in recommendedApps.OrderBy(n => n.Name).Where(n => n.ShouldShow && n.Name != defaultApp.Name))
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, appinfo.GetIcon(), appinfo.Name, true).RegisterWidget());
        using var apps = GAppInfo.GetAllApps();
        foreach (var appinfo in apps.OrderBy(n => n.Name).Where(n => n.ShouldShow))
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, appinfo.GetIcon(), appinfo.Name).RegisterWidget());
        var ela = stopuhr.Elapsed;
        Console.WriteLine(ela);
    }

    [Widget]
    readonly ListBox listbox = null!;


}

