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
    21: Example 1
    22: Example 2
    23: Example 3
    24: Example 4
    25: Example 5
    26: Example 6
    27: Example 7
    28: Example 8
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
    "21" => Example1.Run(),
    "22" => Example2.Run(),
    "23" => Example3.Run(),
    "24" => Example4.Run(),
    "25" => Example5.Run(),
    "26" => Example6.Run(),
    "27" => Example7.Run(),
    "28" => Example8.Run(),
    _ => 0
}}");

// TODO ListStore splice with unref
// TODO gtk_list_view_scroll_to for Ins
// TODO NoSelection, MultiSelection
// TODO ObjectFloatingWin remove, set as child swt floating in ObjectHandle
// TODO Handles implement Interfaces which are partial implemented

// TODO Subclassing with attributes??

// TODO Dark theme, light theme: https://github.com/jbenner-radham/rust-gtk4-css-styling
