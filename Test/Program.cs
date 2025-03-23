using static System.Console;

WriteLine(
    """
    Choose to run:
    1:  First
    2:  Hello World
    3:  Packing buttons
    4:  Bindings
    5:  Drawing
    6:  Builder
    7:  Builder from .NET resource
    8:  Children
    9:  Image
    10: Web View
    11: Web View extended
    12: CSS
    13: Progress
    14: Object subclass
    15: Headerbar with menu (subclassed)
    16: Progress (subclassed)
    17: NotDecorated
    18: Threading
    19: Cleanup
    20: Non GTK
    21: ListView (String List)
    22: ListView (Custom Objects)
    23: ColumnView
    24: ColumnViewControl
    25: CustomColumnView
    26: Example 1
    27: Example 2
    28: Example 3
    29: Example 4
    30: Example 5
    31: Example 6
    32: Example 7
    33: Example 8
    34: Test 
    <any>: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    "2" => HelloWorld.Run(),
    "3" => PackingButtons.Run(),
    "4" => BindingsApp.Run(),
    "5" => Drawing.Run(),
    "6" => BuilderProgram.Run(),
    "7" => ResourceBuilderProgram.Run(),
    "8" => Children.Run(),
    "9" => ImageView.Run(), 
    "10" => Web.Run(),
    "11" => WebExtended.Run(),
    "12" => Css.Run(),
    "13" => Progress.Run(),
    "14" => SubClassing.Run(),
    "15" => MenuSubclass.Run(),
    "16" => ProgressSubclass.Run(),
    "17" => NotDecorated.Run(),
    "18" => Threading.Run(),
    "19" => Cleanup.Run(),
    "20" => NonGtkApp.Run(),
    "21" => StringListView.Run(),
    "22" => CustomItemListView.Run(),
    "23" => ColumnViewApp.Run(),
    "24" => ColumnViewControlApp.Run(),
    "25" => CustomColumnViewApp.Run(),
    "26" => Example1.Run(),
    "27" => Example2.Run(),
    "28" => Example3.Run(),
    "29" => Example4.Run(),
    "30" => Example5.Run(),
    "31" => Example6.Run(),
    "32" => Example7.Run(),
    "33" => Example8.Run(),
    "34" => TestApp.Run(),
    _ => 0
}}");


// TODO Dark theme, light theme: https://github.com/jbenner-radham/rust-gtk4-css-styling

// TODO ColumnView: https://discourse.gnome.org/t/tips-to-initialize-gtk4s-columnview-python/19028
// https://toshiocp.github.io/Gtk4-tutorial/sec32.html
