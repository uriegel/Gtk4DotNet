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
    16: Sub Classing
    17: Example 1
    18: Example 2
    19: Example 3
    20: Example 4
    21: Example 5
    22: Example 6
    23: Example 7
    24: Example 8
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
    "16" => SubClassing.Run(),
    "17" => Example1.Run(),
    "18" => Example2.Run(),
    "19" => Example3.Run(),
    "20" => Example4.Run(),
    "21" => Example5.Run(),
    "22" => Example6.Run(),
    "23" => Example7.Run(),
    "24" => Example8.Run(),
    _ => 0
}}");
