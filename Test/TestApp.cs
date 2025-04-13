using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.Controls;
using static GtkDotNet.Controls.ColumnViewSubClassed;
using CsTools.Extensions;

static class TestApp
{
    public static int Run()
    {
        var res = Application
           .NewAdwaita("de.uriegel.first")
               .OnActivate(app =>
                   app
                       .SubClass(ManagedApplicationWindowClass.Register(p => new AppWindow(p), "customcolumnview"))
                       .SubClass(new CustomColumnViewClass())
                       .ManagedApplicationWindow()
                       .Show())
               .Run(0, IntPtr.Zero);
        Console.WriteLine("Finished");
        return res;
    }

    class AppWindow(nint obj) : ManagedApplicationWindow(obj)
    {
        protected override void OnCreate()
            => Handle.InitTemplate();

        protected override void Initialize()
        {
            Handle.OnSizeChanged((w, h) => Console.WriteLine($"On Size {w}, {h}"));
            var cv = Handle.GetTemplateChild<ColumnViewHandle, WindowHandle>("columnview");
            var columnView = CustomColumnView.GetInstance(cv?.GetInternalHandle() ?? 0) as CustomColumnView;
            Handle.AddActions([new GtkAction("down", () =>
                {
                    Console.WriteLine("Down");
                    // TODO WidgetHandle or ColumnViewRowHandle
                    // g_type_name
                    var widget = Handle.GetFocus<WidgetHandle>();
                    if (!widget.IsInvalid && widget.GetName() == "GtkColumnViewRowWidget")
                    {
                        var next = widget.GetNextSibling<WidgetHandle>();
                        if (!next.IsInvalid && next.GetName() == "GtkColumnViewRowWidget")
                        {
                            var sibling = widget.GetNextSibling<WidgetHandle>();
                            if (!sibling.IsInvalid)
                                sibling.GrabFocus();
                        }
                    }
                }, "Down")]);
            Handle.AddActions([new GtkAction("Ins", () =>
                {
                    Console.WriteLine("Ins");
                    var widget = Handle.GetFocus<WidgetHandle>();
                    if (!widget.IsInvalid && widget.GetName() == "GtkColumnViewRowWidget")
                    {

                        var item = widget.GetFirstChild<WidgetHandle>();
                        var name1 = item.GetName();
                        var listItem = item.GetFirstChild<WidgetHandle>();

                        var name2 = listItem.GetName();
                        // var typ = listItem.GetManagedObjectData<Type2>("data");
                        // Console.WriteLine($"Item: {typ?.EMail}");

                        var data = listItem.GetData("data");
                        var pos = columnView?.FindPos(data);
                        Console.WriteLine($"Pos : {pos}");
                        if (pos.HasValue)
                            columnView?.SelectItem(pos.Value, false);

                        var next = widget.GetNextSibling<WidgetHandle>();
                        if (!next.IsInvalid && next.GetName() == "GtkColumnViewRowWidget")
                            widget?.GetNextSibling<WidgetHandle>()?.GrabFocus();
                    }
                }, "Insert")]);
            Handle.AddController(EventControllerFocus.New().OnEnter(() => Console.WriteLine("Entering")));
            Handle.AddController(
                EventControllerKey
                .New()
                .OnRawKeyPressed((i, c, m) => false.SideEffect(_ => Console.WriteLine($"Key pressed {i}, {c}, {m}")))
                .OnRawKeyReleased((i, c, m) => Console.WriteLine($"Key released {i}, {c}, {m}"))
                .OnModifiers((m) => Console.WriteLine($"Modifiers {m} {m.HasFlag(KeyModifiers.Control)}, {m.HasFlag(KeyModifiers.Alt)}, {m.HasFlag(KeyModifiers.Shift)}")));
        }

        protected override void OnFinalize() => Console.WriteLine("Window finalized");
    }

    class CustomColumnViewClass()
        : ColumnViewSubClassedClass("ColumnView", p => new CustomColumnView(p))
    { }

    class CustomColumnView(nint obj) : ColumnViewSubClassed(obj)
    {
        public int FindPos(nint item)
        {
            Console.WriteLine($"Focused item: {controller.GetFocusedItemPos()}");
            var model = columnView.GetModel<SelectionHandle>();
            var items = model.GetRawItems();
            return items.TakeWhile(n => n != item).Count();
        }

        protected override void OnCreate()
        {
            SetController(controller);
            controller.Fill();
        }

        protected override void OnFinalize()
        {
            Console.WriteLine("ColumnView finalized");
        }
        protected override CustomColumnViewHandle CreateHandle(nint obj) => new(obj);

        static readonly Controller controller = new();
    }
}

class Controller : Controller<Type2>
{
    public Controller()
        => EnableRubberband = true;

    public override Column<Type2>[] GetColumns()
        => [ new()
                {
                    Title = "E Mail",
                    Expanded = true,
                    OnItemSetup = OnIconName,
                    OnItemBind = OnIconNameBind,
                    OnSort = (a, b, d) => string.Compare(a.EMail, b.EMail)
                },
            new()
                {
                    Title = "ID", OnLabelBind = i => i.Id
                },
            new()
                {
                    Title = "Active", OnLabelBind = i => i.Active ? "Yes" : "No",
                    OnSort = (a, b, d) => a.Active.CompareTo(b.Active)
                },
            ];

    public void Fill() => Insert([
        .. Enumerable.Range(1, 100).Select(n => new Type2($"item{n}@dom.de", $"ID-{n}", n % 3 == 0))]);

    static BoxHandle OnIconName()
        => Box
            .New(Orientation.Horizontal)
            .Append(Image.NewFromIconName("mail", IconSize.Button))
            .Append(Label.New().HAlign(Align.Start).MarginStart(5));

    static void OnIconNameBind(ListItemHandle listItem, Type2 item)
    {
        var box = listItem.GetChild<BoxHandle>();
        var image = box.GetFirstChild<ImageHandle>();
        var label = image.GetNextSibling<LabelHandle>();
        if (item.Active)
            image.SetFromIconName("mail-read", IconSize.LargeToolbar);
        else
            image.SetFromIconName("mail-unread", IconSize.LargeToolbar);
        label.Set(item.EMail);
        var itemHandle = listItem.GetRawItem();
        box.SetData("data", itemHandle);
    }
}

//         void InitStore(ApplicationHandle _)
//         {
//             var model1 = ListStore
//                             .New(GContact.GType)
//                             .Splice([.. Enumerable.Range(1, 10).Select(n => GContact.New(new($"Left Item no {n}", $"person{n}@hotmail.de", n)).Handle)]);
//             model2 = ListStore
//                             .New(GContact.GType)
//                             .Append(GContact.New(new("Uwe Riegel", "uriegel@hotmail.de", 1965)))
//                             .Append(GContact.New(new("Jim Doe", "jdoe@hotmail.de", 1955)))
//                             .Append(GContact.New(new("Jane Doe", "jadoe@hotmail.de", 2001)))
//                             .Splice(3, [.. Enumerable.Range(1, 5000).Select(n => GContact.New(new($"Right Item no {n}", $"person{n}@hotmail.de", n)).Handle)]);
//             itemNameFactory = SignalListItemFactory
//                 .New()
//                 .Setup(OnListItemSetup)
//                 .Bind(OnListItemBind)
//                 .Unbind(OnListItemUnbind);
//             itemEMailFactory = SignalListItemFactory
//                 .New()
//                 .Setup(OnListItemSetup)
//                 .Bind(OnEMailBind);
//             itemNumberFactory = SignalListItemFactory
//                 .New()
//                 .Setup(OnListItemSetup)
//                 .Bind(OnNumberBind);

//             selectionModel1 = SingleSelection.New(model1);
//         }

//         return Application
//             .NewAdwaita("de.uriegel.first")
//                 .OnActivate(app =>
//                     app
//                         .SubClass(new GContactClass(GTypeEnum.GObject, "Contact", p => new GContact(p)))
//                         .SideEffect(InitStore)
//                         .NewWindow()
//                             .Titlebar(HeaderBar
//                                 .New()
//                                 .PackEnd(ToggleButton.New()
//                                     .Label("Action")
//                                     .OnToggled(ActionEnabled)))
//                             .Title("Hello Gtk👍")
//                             .DefaultSize(800, 300)
//                             .Child(Paned
//                                 .New(Orientation.Horizontal)
//                                 .StartChild(ScrolledWindow
//                                     .New()
//                                     .Policy(PolicyType.Never, PolicyType.Automatic)
//                                     .Child(ColumnView
//                                         .New(selectionModel1!)
//                                         .OnActivate(pos => Console.WriteLine($"       Position {pos}"))
//                                         .AppendColumn(ColumnViewColumn.New("Name", itemNameFactory!).Expand())
//                                         .AppendColumn(ColumnViewColumn.New("E mail", itemEMailFactory!))), true, true)

//                                 .EndChild(ScrolledWindow
//                                     .New()
//                                     .Policy(PolicyType.Never, PolicyType.Automatic)
//                                     .Child(ColumnView
//                                         .New()
//                                         .OnActivate(pos => Console.WriteLine($"       Position {pos}"))
//                                         .Ref(columnView)
//                                         .AppendColumn(ColumnViewColumn.New("Name", itemNameFactory!)
//                                             .Expand()
//                                             .Resizeable()
//                                             .SetSorter(sorter))
//                                         .AppendColumn(ColumnViewColumn.New("E mail", itemEMailFactory!)
//                                             .Resizeable())
//                                         .AppendColumn(ColumnViewColumn.New("Number", itemNumberFactory!)
//                                             .Resizeable()
//                                             .SetSorter(numberSorter))
//                                         .SideEffect(cv =>
//                                             {
//                                                 var sorter = cv.GetSorter();
//                                                 var model = MultiSelection.New(SortListModel.New(model2!, sorter));
//                                                 cv.SetModel(model);
//                                             })
//                                         .AddController(EventControllerFocus.New()
//                                             .OnEnter(() => IActionMap.GetAction("down").SetEnabled(true))
//                                             .OnLeave(() => IActionMap.GetAction("down").SetEnabled(false)))
//                                         ), true, true))
//                             .Show())
//                 .AddActions([new GtkAction("down", () =>
//                 {
//                     Console.WriteLine("Down");
//                     Mach();
//                 }, "Down")])
//                 .AddActions([new GtkAction("tab", () => Console.WriteLine("Tab"), "Tab")])
//                 .SideEffect(a => IActionMap.GetAction("down").SetEnabled(false))
//                 .Run(0, IntPtr.Zero);
//     }

//     static readonly ObjectRef<ColumnViewHandle> columnView = new();

//     static void Mach()
//     {
//         //var listView = columnView.Ref.GetListView();
//     }

//     static int NameCompare(GObjectHandle data1, GObjectHandle data2)
//     {
//         var c1 = data1.GetInstance() as GContact;
//         var c2 = data2.GetInstance() as GContact;
//         return string.Compare(c1?.Contact?.Name, c2?.Contact?.Name);
//     }

//     static int NumberCompare(GObjectHandle data1, GObjectHandle data2)
//     {
//         var c1 = data1.GetInstance() as GContact;
//         var c2 = data2.GetInstance() as GContact;
//         return (c1?.Contact?.Number ?? 0) > (c2?.Contact?.Number ?? 0) ? 1 : -1;
//     }

//     static void ActionEnabled(ToggleButtonHandle toggleButton)
//         => IActionMap.GetAction("down").SetEnabled(toggleButton.Active());

//     static void OnListItemSetup(ListItemHandle listItem)
//         => listItem.SetChild(Label.New().HAlign(Align.Start));

//     static void OnListItemTearDown(ListItemHandle listItem)
//     {
//         Console.WriteLine("On tear down");
//     }

//     static void OnListItemBind(ListItemHandle listItem)
//     {
//         var label = listItem.GetChild<LabelHandle>();
//         var controller = EventControllerFocus.New()
//             .OnEnter(() =>
//             {
//             //     var data = label.GetData("item");
//             //     var model = columnView.Ref?.GetModel<MultiSelectionHandle>();
//             //     if (model != null)
//             //         model.IsFloating = true;
//             //     var items = model?.GetItems<GObjectHandle>();
//             //     var pos = items?.TakeWhile(n => n.GetInternalHandle() != data).Count() ?? -1;
//             //    Console.WriteLine($"Aktiv: {pos}");
//             });

//         label.GetParent()?.GetParent()?.AddController(controller);
//         label.SetData("controller", controller.GetInternalHandle());

//         var oh = listItem.GetItem<GObjectHandle>();
//         oh.IsFloating = true;
//         label.SetData("item", oh.GetInternalHandle());// TODO or ListItemHandle
//         var item = oh.GetInstance() as GContact;
//         label.Set(item?.Contact?.Name);
//     }

//     static void OnListItemUnbind(ListItemHandle listItem)
//     {
//         var label = listItem.GetChild<LabelHandle>();
//         var controller = new EventControllerFocusHandle
//         {
//             IsFloating = false
//         };
//         controller.SetInternalHandle(label.GetData("controller"));
//         label.GetParent()?.GetParent()?.RemoveController(controller);
//     }

//     static readonly ObjectRef<ColumnViewHandle> listViewRef = new();

//     static IListModel? model2;
//     static CustomSorterHandle sorter = CustomSorter.New<GObjectHandle>(NameCompare);
//     static CustomSorterHandle numberSorter = CustomSorter.New<GObjectHandle>(NumberCompare);
//     static SignalListItemFactoryHandle? itemNameFactory;
//     static SignalListItemFactoryHandle? itemEMailFactory;
//     static SignalListItemFactoryHandle? itemNumberFactory;
//     static SingleSelectionHandle? selectionModel1;

//     static void OnEMailBind(ListItemHandle listItem)
//     {
//         var label = listItem.GetChild<LabelHandle>();
//         var oh = listItem.GetItem<GObjectHandle>();
//         oh.IsFloating = true;
//         var item = oh.GetInstance() as GContact;
//         label.Set(item?.Contact?.EMail);
//     }

//     static void OnNumberBind(ListItemHandle listItem)
//     {
//         var label = listItem.GetChild<LabelHandle>();
//         var oh = listItem.GetItem<GObjectHandle>();
//         oh.IsFloating = true;
//         var item = oh.GetInstance() as GContact;
//         label.Set($"{item?.Contact?.Number}");
//     }
// }
