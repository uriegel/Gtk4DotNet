using System.Diagnostics;
using CsTools.Extensions;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        var store = ListStore.New();
        var items = Enumerable
            .Range(0, 100)
            .Select(n => new Item(n + 1));
        foreach (var item in items)
            store.Append(item);

        var model = SingleSelection.New(store);
        var factory = SignalListItemFactory.New();
        factory.Setup(listitem =>
        {
            listitem.SetChild(Label.New());
        });
        factory.Bind(listitem =>
        {
            var label = listitem.GetChild<Label>();
            var item = listitem.GetItem<Item>();
            label.Text = $"Item #{item?.Number}";
        });

        listview.SetModel(model);
        listview.SetFactory(factory);

        OnFinalize(() =>
        {
            factory.Dispose();
            model.Dispose();
        });
    }

    [Widget]
    readonly ListView listview = null!;
}

record Item(int Number);
