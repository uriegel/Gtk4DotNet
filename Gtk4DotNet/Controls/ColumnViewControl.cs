using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet.Controls;

public class ColumnViewControl
{

    // TODO Filter
    // TODO Filter: remove delegate
    public ScrolledWindowHandle CreateView(Action<ColumnViewControl> onCreated)
    {
        handle = ColumnView.New();
        handle.AddWeakRef(Release);

        onCreated(this);

        scrolledWindow = ScrolledWindow
            .New()
            .Policy(PolicyType.Never, PolicyType.Automatic)
            .Child(handle);
        return scrolledWindow;
    }

    public ColumnViewControl MultiSelection()
    {
        multiSelection = true;
        return this;
    }

    public void SetColumns<T>(ColumnViewControlColumn<T>[] columns, ObservableModel<T> items)
    {
        if (scrolledWindow != null)
        {
            scrolledWindow.RemoveChild();
            handle?.Dispose();
            handle = ColumnView.New();
            handle.AddWeakRef(Release);
            scrolledWindow.Child(handle);
        }

        this.columns.ForEach(h =>
        {
            handle?.RemoveColumn(h);
            h.Dispose();
        });
        this.columns.Clear();

        var type = typeof(T);
        var objectName = "GManagedObjectClass" + type.Name;
        if (!registeredObjects.ContainsKey(objectName))
            registeredObjects.Add(objectName, new GManagedObjectClass<T>(objectName, p => new GManagedObject<T>(p)));

        foreach (var col in columns)
        {
            var itemFactory = SignalListItemFactory
                .New()
                .AddWeakRef(() => Console.WriteLine("itemFactory disposed"))
                .Setup(listItem => listItem.SetChild(col.OnItemSetup()))
                .Bind(listItem =>
                    {
                        var oh = listItem.GetItem<GObjectHandle>();
                        oh.IsFloating = true;
                        if (oh.GetInstance() is GManagedObject<T> item && item.Value != null)
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
            if (col.OnSort != null)
            {
                var sorter = CustomSorter.New<GObjectHandle>((a, b)
                    => a.GetInstance() is GManagedObject<T> t1 && t1.Value != null && b.GetInstance() is GManagedObject<T> t2 && t2.Value != null
                        ? col.OnSort(t1.Value, t2.Value)
                        : 0);
                colHandle.SetSorter(sorter);
                sorters.Add(sorter);
            }
            this.columns.Add(colHandle);
            handle?.AppendColumn(colHandle);
        }

        var model = ListStore
            .New(GManagedObject<T>.GType)
            .Splice([.. items.Items.Select(n => GManagedObject<T>.New(n).Handle)]);

        if (handle != null)
        {
            var sorter = handle.GetSorter();
            var sortListModel = SortListModel.New(model, sorter);
            IListModel selModel = multiSelection ? GtkDotNet.MultiSelection.New(sortListModel) : SingleSelection.New(sortListModel);
            listModelHandle = model;
            handle.SetModel(selModel);
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
        //TODO Remove eventhandlers
        //TODO Sample(), to ui thread
        //TODO clear it here
        //TODO clear it onweakref from this class
    }

    public IEnumerable<T> Items<T>()
    {
        if (handle != null)
        {
            uint pos = 0;
            var model = handle.GetModel<SelectionHandle>();
            while (true)
            {
                var oh = model.GetItem<GObjectHandle>(pos++);
                if (!oh.IsInvalid && oh.GetInstance() is GManagedObject<T> item && item != null && item.Value != null)
                    yield return item.Value;
                else
                    break;
            }
        }
    }

    void Release()
    {
        columns.ForEach(h => h.Dispose());
        if (listModelHandle?.IsFloating != null)
            listModelHandle.IsFloating = false;
        listModelHandle?.Dispose();
        listModelHandle = null;
        columns.Clear();
        sorters.ForEach(h => h.Dispose());
        sorters.Clear();
    }        

    ScrolledWindowHandle? scrolledWindow;
    static readonly Dictionary<string, object> registeredObjects = [];
    readonly List<ColumnViewColumnHandle> columns = [];
    readonly List<CustomSorterHandle> sorters = [];
    ObjectHandle? listModelHandle;
    ColumnViewHandle? handle;
    bool multiSelection;
}

