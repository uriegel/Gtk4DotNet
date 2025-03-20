using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet.Controls;

public class ColumnViewSubClassedClass(string name, Func<nint, ColumnViewSubClassed> constructor)
    : SubClass<CustomColumnViewHandle>(GTypeEnum.ScrolledWindow, name, constructor)
{ }

public abstract class ColumnViewSubClassed : SubClassInst<CustomColumnViewHandle>
{
    public bool SortDescending { get; private set;  }
    public bool MultiSelection { get; set; }

    public ColumnViewSubClassed(nint obj) : base(obj)
    {
        columnView = ColumnView.New();
        Handle.Policy(PolicyType.Never, PolicyType.Automatic);
        Handle.Child(columnView);
        columnView.AddWeakRef(Release);
    }

    public void SetController<T>(Controller<T> controller)
        where T : class
    {
        controller.RemoveAll();
        var model = SetColumns(controller.GetColumns(), controller);
        controller.SetModel(model);
        columnView.EnableRubberband(controller.EnableRubberband);
    }

    public void OnActivate(Action<uint>? onActivate)
    {
        if (onActivate != null)
            columnView.OnActivate(onActivate);
        // TODO Signal disconnect when onActivate == null
    }

    IColumnViewModel<T> SetColumns<T>(Column<T>[] columns, Controller<T> controller)
        where T : class
    {
        this.columns.ForEach(h =>
        {
            columnView.RemoveColumn(h);
            h.Dispose();
        });
        this.columns.Clear();

        sorters.ForEach(h => h.Dispose());
        sorters.Clear();

        foreach (var col in columns)
        {
            var itemFactory = SignalListItemFactory
                .New()
                .AddWeakRef(() => Console.WriteLine("itemFactory disposed"))
                .Setup(listItem => listItem.SetChild(col.OnItemSetup()))
                .Bind(listItem =>
                    {
                        var item = listItem.GetObject<T>();
                        if (item != null)
                        {
                            if (col.OnItemBind != null)
                                col.OnItemBind.Invoke(listItem, item);
                            else if (col.OnLabelBind != null)
                            {
                                var label = listItem.GetChild<LabelHandle>();
                                label.Set(col.OnLabelBind.Invoke(item));
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
                var sorter = CustomSorter.New((a, b) =>
                {
                    var itemA = GetItem(a);
                    var itemB = GetItem(b);
                    return itemA != null && itemB != null
                        ? col.OnSort(itemA, itemB, SortDescending)
                        : 0;
                }).SideEffect(n => n.AddWeakRef(() => Console.WriteLine("Sorter finalized")));

                colHandle.SetSorter(sorter);
                sorters.Add(sorter);
            }
            this.columns.Add(colHandle);
            columnView.AppendColumn(colHandle);
        }

        if (listModelHandle == null)
        {
            var model = ListStore.New();

            filterHandle = CustomFilter.New(OnFilter);
            var sortListModel =
                SortListModel.New(FilterListModel.New(model, filterHandle), columnView.GetSorter().OnChanged((desc, changed) => SortDescending = desc));

            SelectionHandle selModelHandle = MultiSelection ? GtkDotNet.MultiSelection.New(sortListModel) : SingleSelection.New(sortListModel);
            IListModel selModel = selModelHandle;

            // TODO Check Single button-press without ctrl and one selection unselect
            // TODO manual set selection: will it be detected? Yes!

            // TODO implement 
            selModelHandle.OnSelectionChanged((n, p, c) =>
            {
                if (!DontUnselect)
                    n.UnselectRange(p, c);
            });
                
            listModelHandle = model;
            columnView.SetModel(selModel);
        }

        onfilter = item => controller.OnFilter == null || GetItem(item) is T t && t != null && controller.OnFilter!(t);

        return new Model<T>(columnView, listModelHandle);

        T? GetItem(nint h)
        {
            var ptr = h.GetData(ListItem.MANAGED_OBJECT);
            var gcHandle = GCHandle.FromIntPtr(ptr);
            return gcHandle.Target as T;
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
    // TODO eliminate
public static bool DontUnselect { get; set; }
    public void SelectItem(uint pos, bool unselectRest)
        => columnView.GetModel<SelectionHandle>().SelectItem(pos, unselectRest);

    public void FilterChanged(FilterChange change)
        => filterHandle?.Changed(change);

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
        public Func<WidgetHandle> OnItemSetup { get; set; } = () => Label.New().HAlign(Align.Start);
        public Action<ListItemHandle, TObj>? OnItemBind { get; set; }
        public Func<TObj, string>? OnLabelBind { get; set; }
        public Func<TObj, TObj, bool, int>? OnSort { get; set; }
    }

    public abstract class Controller<T>
        where T : class
    {
        public bool EnableRubberband { get; set; }
        public Func<T, bool>? OnFilter { get; set; }

        public abstract Column<T>[] GetColumns();
        public void Insert(IEnumerable<T> items) => model?.Insert(items);
        public void RemoveAll() => model?.RemoveAll();
        public void Insert(uint pos, IEnumerable<T> items) => model?.Insert(pos, items);

        public IEnumerable<T> Items() => model?.Items() ?? [];

        public T? GetItem(uint pos) => model?.GetItem(pos);

        internal void SetModel(IColumnViewModel<T> model)
            => this.model = model;

        IColumnViewModel<T>? model;
    }

    class EmptyController : Controller<object>
    {
        public override Column<object>[] GetColumns() => [];
    }

    class Model<T>(ColumnViewHandle columnView, IListModel? listModelHandle) : IColumnViewModel<T>
        where T : class
    {
        public IEnumerable<T> Items()
        {
            uint pos = 0;
            var model = columnView.GetModel<SelectionHandle>();
            while (true)
            {
                var t = model.GetItem<T>(pos++);
                if (t != null)
                    yield return t;
                else
                    break;
            }
        }
        public void Insert(IEnumerable<T> items)
        {
            listModelHandle?.RemoveAll();
            listModelHandle?.Splice(items);
        }

        public void Insert(uint pos, IEnumerable<T> items)
        {
            listModelHandle?.RemoveAll();
            listModelHandle?.Splice(pos, items);
        }

        public void RemoveAll()
            => listModelHandle?.RemoveAll();

        public T? GetItem(uint pos) => columnView.GetModel<SelectionHandle>().GetItem<T>(pos++);
    }

    protected ColumnViewHandle columnView = new(0);
    static readonly Dictionary<string, object> registeredObjects = [];
    readonly List<ColumnViewColumnHandle> columns = [];
    readonly List<CustomSorterHandle> sorters = [];

    bool OnFilter(nint item) => onfilter(item);

    Func<nint, bool> onfilter = _ => true;
    IListModel? listModelHandle;
    CustomFilterHandle? filterHandle;
}
