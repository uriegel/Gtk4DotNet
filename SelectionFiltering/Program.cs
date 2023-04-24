using GtkDotNet;

// TODO SelectionModel

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    Application.RegisterResources();
    var cssProvider = CssProvider.New();
    cssProvider.CssProviderLoadFromResource("/org/gtk/example/style.css");
    StyleContext.AddProviderForDisplay(Display.GetDefault(), cssProvider, StyleProviderPriority.Application);

    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    var columnView = Builder.GetObject(builder, "column-view");
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
        var item = listStore.GetListItem(listItem);
        var child = ListItem.GetChild(listItem);
        Label.SetLabel(child, $"Eintrag # {item}");

        var row = Widget.GetParent(Widget.GetParent(child));
        if (item == "Item # 10")
            Widget.AddCssClass(row, "farbig");
        else
            Widget.RemoveCssClass(row, "farbig");
    });

    CustomSorter.CompareDelegate test = (a, b, z) =>
    {
        var text = GManaged<string>.GetValue(a);        
        var textB = GManaged<string>.GetValue(b);        
        return string.Compare(text, textB);
    };

    var sorter = CustomSorter.New(test);
    var column = ColumnViewColumn.New("Spalte 1", modelFactory);
    ColumnViewColumn.SetResizable(column, true);
    ColumnView.AppendColumn(columnView, column);
    column = ColumnViewColumn.New("Spalte 2", modelFactory);
    ColumnView.AppendColumn(columnView, column);
    ColumnViewColumn.SetResizable(column, true);
    ColumnViewColumn.SetSorter(column, sorter);
    ColumnView.SetReorderable(columnView, true);

    var sortModel = SortListModel.New(listStore, ColumnView.GetSorter(columnView));
    var selectionModel = MultiSelection.New(sortModel);
    ColumnView.SetModel(columnView, selectionModel);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;


