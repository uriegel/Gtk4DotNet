using System.Collections.ObjectModel;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet.Controls;

public class ColumnViewControl
{
    public ScrolledWindowHandle CreateView(Action<ColumnViewControl> onCreated)
    {
        handle = ColumnView.New();
        handle.AddWeakRef(() =>
            {
                columns.ForEach(h => h.Dispose());
                if (listModelHandle?.IsFloating != null)
                    listModelHandle.IsFloating = false;
                listModelHandle?.Dispose();
            });

        onCreated(this);

        return ScrolledWindow
            .New()
            .Policy(PolicyType.Never, PolicyType.Automatic)
            .Child(handle);
    }

    public void SetColumns<T>(ColumnViewControlColumn<T>[] columns, ObservableCollection<T> items)
    {
        this.columns.ForEach(h =>
        {
            h.Dispose();
            handle?.RemoveColumn(h);
        });
        this.columns.Clear();

        listModelHandle?.Dispose();

        var type = typeof(T);
        var objectName = "GManagedObjectClass" + type.Name;
        if (!registeredObjects.ContainsKey(objectName))
            registeredObjects.Add(objectName, new GManagedObjectClass<T>(objectName, p => new GManagedObject<T>(p)));

        foreach (var col in columns)
        {
            var itemFactory = SignalListItemFactory
                .New()
                .AddWeakRef(() => Console.WriteLine("itemFactory disposed"))
                .Setup(listItem => listItem.SetChild(Label.New("").HAlign(Align.Start)))
            // TODO custom setup
            //TODO.Bind(OnListItemBind);
                .Bind(listItem =>
                    {
                        var oh = listItem.GetItem<GObjectHandle>();
                        oh.IsFloating = true;
                        var item = oh.GetInstance() as GManagedObject<T>;
                        if (item != null && item.Value != null)
                        {
                            if (col.OnItemBind != null)
                                col.OnItemBind.Invoke(listItem, item.Value);
                            else if (col.OnLabelBind != null)
                            {
                                var label = listItem.GetChild<LabelHandle>();
                                label.Set(col.OnLabelBind.Invoke(item.Value));
                            }
                        }
                    });
            var colHandle = ColumnViewColumn.New(col.Title, itemFactory)
                .AddWeakRef(() => Console.WriteLine("ColumnViewColumn finalized"));
            if (col.Expanded)
                colHandle.Expand();
            if (col.Resizeable)
                colHandle.Resizeable();
            //                .SetSorter(sorter);

            this.columns.Add(colHandle);
            handle?.AppendColumn(colHandle);
        }

        var model = ListStore
            .New(GManagedObject<T>.GType)
            .Splice([.. items.Select(n => GManagedObject<T>.New(n).Handle)]);

        if (handle != null)
        {
            // var sorter = handle.GetSorter();
            // var selModel = MultiSelection.New(SortListModel.New(model, sorter));
            // TODO single or multi
            var selModel = MultiSelection.New(model);
            handle.SetModel(selModel);
            listModelHandle = selModel;
        }



        //  class ObservableModel<T>(): IDisposable
        // {
        //     public ObservableCollection<T> Items 
        //     ObservableCollection it
        //         remove eventhandler on dispose

        //     }
        //     // TODO attach ObservableCollection<T> 
        // TODO save it in control as objectx
        //TODO clear it so that all GObjects can be disposed
        //TODO clear it here
        //TODO clear it onweakref from this class
    }

    // TODO Bounds.cs with saving in App.Settings

    static Dictionary<string, object> registeredObjects = new();
    List<ColumnViewColumnHandle> columns = new();
    ObjectHandle? listModelHandle;
    ColumnViewHandle? handle;
}


