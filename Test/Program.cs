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
    12: Progress
    13: Object subclass
    14: Headerbar with menu (subclassed)
    15: Progress (subclassed)
    16: NotDecorated
    17: Threading
    18: Cleanup
    19: Non GTK
    20: ListView (String List)
    21: ListView (Custom Objects)
    22: ColumnView
    23: 2 ColumnView 
    24: ColumnView changing
    25: ColumnViewControl
    26: CustomColumnView
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
    "12" => Progress.Run(),
    "13" => SubClassing.Run(),
    "14" => MenuSubclass.Run(),
    "15" => ProgressSubclass.Run(),
    "16" => NotDecorated.Run(),
    "17" => Threading.Run(),
    "18" => Cleanup.Run(),
    "19" => NonGtkApp.Run(),
    "20" => StringListView.Run(),
    "21" => CustomItemListView.Run(),
    "22" => ColumnViewApp.Run(),
    "23" => TwoColumnViews.Run(),
    "24" => ChangingColumnViews.Run(),
    "25" => ColumnViewControlApp.Run(),
    "26" => CustomColumnViewApp.Run(),
    "27" => Example1.Run(),
    "28" => Example2.Run(),
    "29" => Example3.Run(),
    "30" => Example4.Run(),
    "31" => Example5.Run(),
    "32" => Example6.Run(),
    "33" => Example7.Run(),
    "34" => Example8.Run(),
    "35" => TestApp.Run(),
    _ => 0
}}");

// TODO: Implement this all in Commander:

// TODO Focused Item with gray Border: css
// Forget TODO  Inherit MultiSelectionModel to adapt in another custom inheritance:
// Forget TODO The abstract multiselection is suplied with keyboard and mouse events from ColumnView
// Forget TODO The abstract multiselection translates C style selections to C# comfort selections
// TODO Single Selection via arrow up/down, call set_selection manually
// TODO Switch to multi selection when more than two items are selected
// TODO Focus Border always:
// row:focus {
//     /*outline: 1px solid red; */ /* Force a visible focus outline */
// 	border-width: 1px;
// 	border-style: solid;
// }


// TODO Dark theme, light theme: https://github.com/jbenner-radham/rust-gtk4-css-styling

// TODO ColumnView: https://discourse.gnome.org/t/tips-to-initialize-gtk4s-columnview-python/19028
// https://toshiocp.github.io/Gtk4-tutorial/sec32.html
