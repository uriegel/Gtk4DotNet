using System.Diagnostics;
using CsTools.Extensions;
using Gtk4DotNet;

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

        var keyController = KeyEventController.New();
        keyController.OnKeyPressed((chr, mod) =>
        {
            if (chr == 13)
            {
                var row = listbox.GetSelectedRow().GetChild<Box>();
                Console.WriteLine($"Open file with {row?.GetManagedData<string>("data")}");
                return true;
            }
            return false;
        });
        AddController(keyController);

        EventController CreatePressed() => ClickGesture.New().SideEffect(c => c.OnPressed((n, x, y) =>
        {
            if (n == 2)
            {
                var row = listbox.GetSelectedRow().GetChild<Box>();
                Console.WriteLine($"Open file with {row?.GetManagedData<string>("data")}");
            }
        }));

        var contentType = Gio.GuessContentType(".html") ?? "none";
        using var defaultApp = GAppInfo.GetDefault(contentType);
        if (defaultApp != null)
        {
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, defaultApp.GetIcon(), defaultApp.Name, true)
                .RegisterWidget()
                .SideEffect(n => AttachData(n, defaultApp.Executable)));
        }
        using var recommendedApps = GAppInfo.GetRecommendedApps(contentType);
        foreach (var appinfo in recommendedApps.OrderBy(n => n.Name).Where(n => n.ShouldShow && n.Name != defaultApp?.Name))
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, appinfo.GetIcon(), appinfo.Name, true)
                .RegisterWidget()
                .SideEffect(n => AttachData(n, appinfo.Executable)));
        using var apps = GAppInfo.GetAllApps();
        foreach (var appinfo in apps.OrderBy(n => n.Name).Where(n => n.ShouldShow))
            listbox.AppendFromTemplate("listitem", b => new ListItem(b, appinfo.GetIcon(), appinfo.Name)
                .RegisterWidget()
                .SideEffect(n => AttachData(n, appinfo.Executable)));

        void AttachData(ListItem listItem, string? executable)
        {
            listItem.AddController(CreatePressed());
            listItem.SetManagedData("data", executable ?? "");
        }
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