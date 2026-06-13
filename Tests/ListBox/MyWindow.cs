using System.Diagnostics;
using Gtk4DotNet;

// TODO Test with Finalizer in ListItem
class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        listbox.SetHeaderFunc<ListItem>((current, previous) =>
        {
            var currentListitem = current?.GetChild<ListItem>();
            var previousListitem = previous?.GetChild<ListItem>();
            if (previous == null && currentListitem?.IsRecommended == true)
                current?.CreateHeader("Recommended Apps");
            if (previousListitem?.IsRecommended == true && currentListitem?.IsRecommended == false)
                current?.CreateHeader("All Apps");
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

static class MyWindowExtensions
{
    public static void CreateHeader(this ListBoxRow listBoxRow, string header)
    {
        using var builder = Builder.FromDotNetResource("listitemheader");
        listBoxRow.SetHeader(new ListItemHeader(builder, header));
    }
}