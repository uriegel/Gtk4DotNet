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
    20: Example 1
    21: Example 2
    22: Example 3
    23: Example 4
    24: Example 5
    25: Example 6
    26: Example 7
    27: Example 8
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
    "20" => Example1.Run(),
    "21" => Example2.Run(),
    "22" => Example3.Run(),
    "23" => Example4.Run(),
    "24" => Example5.Run(),
    "25" => Example6.Run(),
    "26" => Example7.Run(),
    "27" => Example8.Run(),
    _ => 0
}}");

// TODO Unref Application
// TODO Perhaps Unref ApplicationWindow
// TODO ObjectFloatingWin remove, set as child swt floating in ObjectHandle
// TODO css aselection style
// TODO keyboard events to make own selections
// TODO CustomObject as listitemmodel