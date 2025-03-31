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
    8:  Image
    9:  Web View
    10: Web View extended
    11: CSS
    12: Bindings
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
    26: Dialogs
    27: Example 1
    28: Example 2
    29: Example 3
    30: Example 4
    31: Example 5
    32: Example 6
    33: Example 7
    34: Example 8
    35: Test 
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
    "8" => ImageView.Run(), 
    "9" => Web.Run(),
    "10" => WebExtended.Run(),
    "11" => Css.Run(),
    "12" => BindingsApp.Run(),
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
    "26" => Dialogs.Run(),
    "27" => Example1.Run(),
    "28" => Example2.Run(),
    "29" => Example3.Run(),
    "30" => Example4.Run(),
    "32" => Example5.Run(),
    "33" => Example6.Run(),
    "34" => Example7.Run(),
    "35" => Example8.Run(),
    "36" => TestApp.Run(),
    _ => 0
}}");


// TODO Dark theme, light theme: https://github.com/jbenner-radham/rust-gtk4-css-styling

// TODO ColumnView: https://discourse.gnome.org/t/tips-to-initialize-gtk4s-columnview-python/19028
// https://toshiocp.github.io/Gtk4-tutorial/sec32.html
