using CsTools;
using GtkDotNet.Controls;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public class ColumnViewSubClassedClass(string name, Func<nint, ColumnViewSubClassed> constructor)
    : SubClass<CustomColumnViewHandle>(GTypeEnum.ScrolledWindow, name, constructor)
{ }

public abstract class ColumnViewSubClassed : SubClassInst<CustomColumnViewHandle>
{
    // TODO Filter
    // TODO Filter: remove delegate
    public ColumnViewSubClassed(nint obj) : base(obj)
    {
        columnView = ColumnView.New();
        Handle.Policy(PolicyType.Never, PolicyType.Automatic);
        Handle.Child(columnView);
        columnView.AddWeakRef(Release);
    }

    public void SetController<T>(Controller<T> controller)
    {
        MultiSelection = controller.MultiSelection;
        var model = SetColumns(controller.GetColumns());
        controller.SetModel(model);
        if (controller.EnableRubberband)
            columnView.EnableRubberband();
    }

    IColumnViewModel<T> SetColumns<T>(Column<T>[] columns)
    {
        Handle.RemoveChild();
        columnView.Dispose();
        columnView = ColumnView.New();
        columnView.AddWeakRef(Release);
        Handle.Child(columnView);

        this.columns.ForEach(h =>
        {
            columnView.RemoveColumn(h);
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
                        if (listItem.GetObject<GManagedObject<T>>() is GManagedObject<T> item && item.Value != null)
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
            columnView.AppendColumn(colHandle);
        }

        var model = ListStore
            .New(GManagedObject<T>.GType);
        //            .Splice([.. items.Items.Select(n => GManagedObject<T>.New(n).Handle)]);

        var sortListModel = SortListModel.New(model, columnView.GetSorter());

        IListModel selModel = MultiSelection ? GtkDotNet.MultiSelection.New(sortListModel) : SingleSelection.New(sortListModel);
        listModelHandle = model;
        columnView.SetModel(selModel);

        return new Model<T>(columnView, listModelHandle);
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

    public void SelectItem(uint pos, bool unselectRest)
        => columnView.GetModel<SelectionHandle>().SelectItem(pos, unselectRest);

    static SubClassInst<CustomColumnViewHandle>? GetInstance(ColumnViewHandle handle)
        => GetInstance(handle.GetInternalHandle());

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
    
    public class Column<TObj>
    {
        public string Title { get; set; } = string.Empty;
        public bool Expanded { get; set; }
        public bool Resizeable { get; set; }
        public Func<WidgetHandle> OnItemSetup { get; set; } = () => Label.New("").HAlign(Align.Start);
        public Action<ListItemHandle, TObj>? OnItemBind { get; set; }
        public Func<TObj, string>? OnLabelBind { get; set; } 
        public Func<TObj, TObj, int>? OnSort { get; set; } 
    }

    public abstract class Controller<T>
    {
        public bool MultiSelection { get; set; }
        public bool EnableRubberband { get; set; }

        public abstract Column<T>[] GetColumns();
        public void Insert(IEnumerable<T> items) => model?.Insert(items);
        public void Insert(uint pos, IEnumerable<T> items) => model?.Insert(pos, items);

        internal void SetModel(IColumnViewModel<T> model)
            => this.model = model;

        IColumnViewModel<T>? model;
    }

    class EmptyController : Controller<Unit>
    {
        public override Column<Unit>[] GetColumns() => [];
    }

    class Model<T>(ColumnViewHandle columnView, IListModel? listModelHandle) : IColumnViewModel<T>
    {
        public IEnumerable<T> Items()
        {
            uint pos = 0;
            var model = columnView.GetModel<SelectionHandle>();
            while (true)
            {
                var oh = model.GetItem<GObjectHandle>(pos++);
                if (!oh.IsInvalid && oh.GetInstance() is GManagedObject<T> item && item != null && item.Value != null)
                    yield return item.Value;
                else
                    break;
            }
        }
        public void Insert(IEnumerable<T> items)
            => listModelHandle?.Splice([.. items.Select(n => GManagedObject<T>.New(n).Handle)]);
        public void Insert(uint pos, IEnumerable<T> items)
            => listModelHandle?.Splice(pos, [.. items.Select(n => GManagedObject<T>.New(n).Handle)]);
    }

    protected ColumnViewHandle columnView = new(0);
    static readonly Dictionary<string, object> registeredObjects = [];
    readonly List<ColumnViewColumnHandle> columns = [];
    readonly List<CustomSorterHandle> sorters = [];
    bool MultiSelection { get; set; }
    IListModel? listModelHandle;
}
