using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet.Controls;

public class ColumnViewSubClassedClass(string name, Func<nint, ColumnViewSubClassed> constructor)
    : SubClass<CustomColumnViewHandle>(GTypeEnum.ScrolledWindow, name, constructor)
{ 
    public const int PROP_TABBEHAVIOR = 1;

    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        RegisterProperty(cls, PROP_TABBEHAVIOR, "tab-behavior");
    }
}

public abstract class ColumnViewSubClassed : SubClassWidgetInst<CustomColumnViewHandle>
{
    public bool SortDescending { get; private set; }
    public bool MultiSelection { get; set; }
    public bool SingleSelection { get; set; }

    public Action<nint, int, int>? OnSelectionChanged { get; set; }

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
        controller.SetModel(model, columnView);
        columnView.EnableRubberband(controller.EnableRubberband);
        columnView.TabBehavior(tabBehavior);
    }

    public void OnActivate(Action<int>? onActivate)
    {
        if (onActivate != null)
            columnView.OnActivate(onActivate);
        // TODO Signal disconnect when onActivate == null
    }

    public void SetTabBehavior(ListTabBehavior behavior)
    {
        tabBehavior = behavior;
        columnView.TabBehavior(behavior);
    }

    public void SetSelection(int start, int count)
    {
        var model = columnView.GetModel<SelectionHandle>();
        if (start == 0 && count == -1)
            model.SelectAll();
        else
            model.SelectRange(start, count, true);
    }

    public void SelectAll()
    {
        var model = columnView.GetModel<SelectionHandle>();
        model.SelectAll();
    }

    public void UnselectAll()
    {
        var model = columnView.GetModel<SelectionHandle>();
        model.UnselectAll();
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
                        Controller<T>.AttachListItem(listItem);
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

            SelectionHandle selModelHandle =
                MultiSelection
                ? GtkDotNet.MultiSelection.New(sortListModel)
                : SingleSelection
                ? GtkDotNet.SingleSelection.New(sortListModel)
                : GtkDotNet.NoSelection.New(sortListModel);
            IListModel selModel = selModelHandle;

            // TODO Check Single button-press without ctrl and one selection unselect: No!!
            // TODO instead: GtkGestureClick in combination with  Display display = listView.Display; Seat seat = display.DefaultSeat; ModifierType modifiers = seat.Pointer.ModifierState;
            // TODO manual set selection: will it be detected? Yes!

            // TODO implement 
            if (OnSelectionChanged != null)
                selModelHandle.OnSelectionChanged(OnSelectionChanged);
                // {
                //     if (!DontUnselect)
                //         n.UnselectRange(p, c);
                // });

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
    public void SelectItem(int pos, bool unselectRest)
        => columnView.GetModel<SelectionHandle>().SelectItem(pos, unselectRest);

    public void FilterChanged(FilterChange change)
        => filterHandle?.Changed(change);

    static SubClassInst<CustomColumnViewHandle>? GetInstance(ColumnViewHandle handle)
        => GetInstance(handle.GetInternalHandle());

    protected override void OnSetProperty(int propId, nint value)
    {
        if (propId == ColumnViewSubClassedClass.PROP_TABBEHAVIOR)
            columnView.TabBehavior((ListTabBehavior)GValue.GetInt(value));
    }

    protected override void OnGetProperty(int propId, nint value)
    {
        if (propId == ColumnViewSubClassedClass.PROP_TABBEHAVIOR)
             GValue.SetInt(value, (int)columnView.GetTabBehavior());
    }

    ListTabBehavior tabBehavior;

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
        public void Insert(int pos, IEnumerable<T> items) => model?.Insert(pos, items);

        public IEnumerable<T> Items() => model?.Items() ?? [];
        public IEnumerable<nint> RawItems() => model?.RawItems() ?? [];

        public T? GetItem(int pos) => model?.GetItem(pos);

        public int GetFocusedItemPos()
        {
            if (window.IsInvalid)
                window = columnView.GetAncestor<WindowHandle>();
            var row = window.GetFocus<WidgetHandle>();
            if (!IsWidgetInColumnView(row))
                return -1;
            if (!row.IsInvalid && row.GetName() == "GtkColumnViewRowWidget")
                {
                    ListItemHandle listItem = new(row.GetData(LISTITEM));
                    var focusedItem = listItem.GetRawItem();
                    return RawItems().TakeWhile(n => n != focusedItem).Count();
                }
                else
                    return -1;
        }

        public int ItemsCount() => RawItems().Count();

        public void SetSelection(int start, int count)
        {
            var model = columnView.GetModel<SelectionHandle>();
            if (start == 0 && count == -1)
                model.SelectAll();
            else
                model.SelectRange(start, count, true);
        }

        public void UnselectAll()
        {
            var model = columnView.GetModel<SelectionHandle>();
            model.UnselectAll();
        }

        static internal void AttachListItem(ListItemHandle listItem)
        {
            var widget = listItem.GetChild<WidgetHandle>();
            var row = widget.GetParent().GetParent();
            if (!row.IsInvalid && row.GetName() == "GtkColumnViewRowWidget")
                row.SetData(LISTITEM, listItem.GetInternalHandle());
        }   

        internal void SetModel(IColumnViewModel<T> model, ColumnViewHandle columnView)
        {
            this.model = model;
            this.columnView = columnView;
        }

        bool IsWidgetInColumnView(WidgetHandle w)
        {
            while (true)
            {
                var p = w.GetParent();
                if (p.IsInvalid)
                    return false;
                if (p.GetInternalHandle() == columnView.GetInternalHandle())
                    return true;
                w = p;
            }
        }

        const string LISTITEM = "LISTITEM";

        IColumnViewModel<T>? model;
        ColumnViewHandle columnView = new();
        WindowHandle window = new();
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
            var pos = 0;
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

        public IEnumerable<nint> RawItems() => columnView.GetModel<SelectionHandle>().GetRawItems();

        public void Insert(IEnumerable<T> items)
        {
            listModelHandle?.RemoveAll();
            listModelHandle?.Splice(items);
        }

        public void Insert(int pos, IEnumerable<T> items)
            => listModelHandle?.Splice(pos, items);

        public void RemoveAll() => listModelHandle?.RemoveAll();

        public T? GetItem(int pos) => columnView.GetModel<SelectionHandle>().GetItem<T>(pos++);

        public void SelectAll()
        {
            var model = columnView.GetModel<SelectionHandle>();
            model.SelectAll();
        }

        public void SetSelection(int start, int count)
        {
            var model = columnView.GetModel<SelectionHandle>();
            model.SelectRange(start, count, true);
        }

        public void UnselectAll()
        {
            var model = columnView.GetModel<SelectionHandle>();
            model.UnselectAll();
        }
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
