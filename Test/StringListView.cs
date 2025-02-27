using GtkDotNet;
using GtkDotNet.SafeHandles;

static class StringListView
{
    public static int Run()
    {
        var model = StringList.New(
                        Enumerable
                            .Range(0, 100_000)
                            //.Range(0, 10)
                            .Select(n => n.ToString())
                            .ToArray());
        var itemFactory = SignalListItemFactory
            .New()
            .Setup(OnListItemSetup)
            .Bind(OnListItemBind);
        var selectionModel = SingleSelection.New(model);

        return Application
        // TODO
                        .NewAdwaita("org.gtk.example")
            //.New("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Hello Gtk👍")
                    .DefaultSize(400, 800)
                    .Child(ScrolledWindow
                        .New()
                        .Policy(PolicyType.Always, PolicyType.Always))
                        .Child(ListView
                            .New(selectionModel, itemFactory))
                    .Show())
            .Run(0, IntPtr.Zero);
    }

    static void OnListItemSetup(ListItemHandle listItem)
    {
        listItem.SetChild(Label.New("Ein Eintrag"));
        Console.WriteLine($"OnSetup {++i}");
    }

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild();
        label.Set("Gesetzt");
        Console.WriteLine($"OnBind {++i}");
    }

    static int i;
}

// import gi
// gi.require_version('Gtk', '4.0')
// from gi.repository import Gtk, Gio

// class ListViewExample(Gtk.Window):
//     def __init__(self):
//         super().__init__(title="GtkListView in ScrolledWindow")
//         self.set_default_size(400, 300)

//         # Create a list store with some sample data
//         self.list_store = Gio.ListStore.new(Gtk.StringObject)
//         for i in range(100):
//             self.list_store.append(Gtk.StringObject.new(f"Item {i}"))

//         # Create a factory to generate list items
//         self.factory = Gtk.SignalListItemFactory()
//         self.factory.connect("setup", self.setup_item)
//         self.factory.connect("bind", self.bind_item)

//         # Create the list view
//         self.list_view = Gtk.ListView(model=self.list_store, factory=self.factory)

//         # Embed the list view in a scrolled window
//         self.scrolled_window = Gtk.ScrolledWindow()
//         self.scrolled_window.set_child(self.list_view)

//         # Add the scrolled window to the main window
//         self.set_child(self.scrolled_window)

//     def setup_item(self, factory, list_item):
//         label = Gtk.Label()
//         list_item.set_child(label)

//     def bind_item(self, factory, list_item):
//         label = list_item.get_child()
//         item = list_item.get_item()
//         label.set_text(item.get_string())

// # Run the application
// app = Gtk.Application(application_id='com.example.GtkListView')
// app.connect('activate', lambda app: ListViewExample().show())
// app.run(None)