using System;
using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    Application.RegisterResources();
    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    var listView = Builder.GetObject(builder, "list-view");
    Window.SetApplication(window, app);
    GObject.Unref( builder);
    Widget.Show(window);

    var listStore = new ListStore<string>();
    listStore.Splice(Enumerable.Range(0, 200_000).Select(n => $"Item # {n}"));
    var val = listStore.GetObject(1);

    var modelFactory = SignalListItemFactory.New();
    Gtk.SignalConnect<SignalListItemFactory.Delegate>(modelFactory, "setup", (_, listItem, _) => 
    {
        var label = Label.New("");
        ListItem.SetChild(listItem, label);
    });
    Gtk.SignalConnect<SignalListItemFactory.Delegate>(modelFactory, "bind", (_, listItem, _) => 
    {
        var item = listStore.GetListItem(ListItem.GetItem(listItem));
        var child = ListItem.GetChild(listItem);
        Label.SetLabel(child, $"Eintrag # {item}");
    });

    var selectionModel = SingleSelection.New(listStore);
    ListView.SetModel(listView, selectionModel);
    ListView.SetFactory(listView, modelFactory);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

