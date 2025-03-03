using static System.Console;

WriteLine(
    """
    Choose to run:
    1:  First
    2:  Hello World
    3:  Packing buttons
    4:  Drawing
    5:  Builder
    6:  Builder from .NET resource
    7:  Children
    8:  Web View
    9:  Web View extended
    10: CSS
    11: Progress
    12: Object subclass
    13: Headerbar with menu (subclassed)
    14: Progress (subclassed)
    15: NotDecorated
    16: Threading
    17: Cleanup
    18: Non GTK
    19: ListView (String List)
    20: ListView (Custom Objects)
    21: ColumnView
    22: 2 ColumnView 
    23: ColumnView changing
    24: Example 1
    25: Example 2
    26: Example 3
    27  Example 4
    28: Example 5
    29: Example 6
    30: Example 7
    31: Example 8
    <any>: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    "2" => HelloWorld.Run(),
    "3" => PackingButtons.Run(),
    "4" => Drawing.Run(),
    "5" => BuilderProgram.Run(),
    "6" => ResourceBuilderProgram.Run(),
    "7" => Children.Run(),
    "8" => Web.Run(),
    "9" => WebExtended.Run(),
    "10" => Css.Run(),
    "11" => Progress.Run(),
    "12" => SubClassing.Run(),
    "13" => MenuSubclass.Run(),
    "14" => ProgressSubclass.Run(),
    "15" => NotDecorated.Run(),
    "16" => Threading.Run(),
    "17" => Cleanup.Run(),
    "18" => NonGtkApp.Run(),
    "19" => StringListView.Run(),
    "20" => CustomItemListView.Run(),
    "21" => ColumnViewApp.Run(),
    "22" => TwoColumnViews.Run(),
    "23" => ChangingColumnViews.Run(),
    "24" => Example1.Run(),
    "25" => Example2.Run(),
    "26" => Example3.Run(),
    "27" => Example4.Run(),
    "28" => Example5.Run(),
    "29" => Example6.Run(),
    "30" => Example7.Run(),
    "31" => Example8.Run(),
    _ => 0
}}");

// TODO Check if Models are disposed
// TODO SubClass<GObjectHandle> => GObject<T>
// TODO Selected Item with red Border
// TODO Dark theme, light theme: https://github.com/jbenner-radham/rust-gtk4-css-styling

// TODO ColumnView: https://discourse.gnome.org/t/tips-to-initialize-gtk4s-columnview-python/19028
// https://toshiocp.github.io/Gtk4-tutorial/sec32.html

// TODO Subclassing with attributes??

