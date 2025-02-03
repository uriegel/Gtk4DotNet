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
    12: NotDecorated
    13: Threading
    14: Cleanup
    15: Non GTK
    16: Example 1
    17: Example 2
    18: Example 3
    19: Example 4
    20: Example 5
    21: Example 6
    22: Example 7
    23: Example 8
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
    "12"=> NotDecorated.Run(),
    "13"=> Threading.Run(),
    "14"=> Cleanup.Run(),
    "15"=> NonGtkApp.Run(),
    "16"=> Example1.Run(),
    "17" => Example2.Run(),
    "18" => Example3.Run(),
    "19" => Example4.Run(),
    "20" => Example5.Run(),
    "21" => Example6.Run(),
    "22" => Example7.Run(),
    "23" => Example8.Run(),
    _ => 0
}}");
