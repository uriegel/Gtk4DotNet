using GtkDotNet;
using GtkDotNet.SafeHandles;

static class StringListView
{
    public static int Run()
    {
        var model = StringList.New(
                        Enumerable
                            .Range(1, 100_000)
                            .Select(n => $"Item no {n}")
                            .ToArray());
        var itemFactory = SignalListItemFactory
            .New()
            .Setup(OnListItemSetup)
            .Bind(OnListItemBind);
        var selectionModel = SingleSelection.New(model);

        return Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Hello Gtk👍")
                    .DefaultSize(400, 600)
                    .Child(ScrolledWindow
                        .New()
                        .Policy(PolicyType.Never, PolicyType.Automatic)
                        .Child(ListView
                            .New(selectionModel, itemFactory)))
                    .Show())
            .Run(0, IntPtr.Zero);
    }

    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New(""));

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var item = listItem.GetItem<StringObjectHandle>();
        label.Set(item.Get());
    }
}
