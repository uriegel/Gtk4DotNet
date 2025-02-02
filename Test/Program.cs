using static System.Console;

WriteLine(
    """
    Choose to run:
    1:  First
    2:  Hello World
    3:  Packing buttons
    4:  Drawing
    5:  Builder
    6:  Children
    7:  Web View
    8:  Web View extended
    9:  CSS
    10: Progress
    11: NotDecorated
    12: Threading
    13: Cleanup
    14: Non GTK
    15: Example 1
    16: Example 2
    17: Example 3
    18: Example 4
    19: Example 5
    20: Example 6
    21: Example 7
    22: Example 8
    <any>: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    "2" => HelloWorld.Run(),
    "3" => PackingButtons.Run(),
    "4" => Drawing.Run(),
    "5" => BuilderProgram.Run(),
    "6" => Children.Run(),
    "7" => Web.Run(),
    "8" => WebExtended.Run(),
    "9" => Css.Run(),
    "10" => Progress.Run(),
    "11"=> NotDecorated.Run(),
    "12"=> Threading.Run(),
    "13"=> Cleanup.Run(),
    "14"=> NonGtkApp.Run(),
    "15"=> Example1.Run(),
    "16" => Example2.Run(),
    "17" => Example3.Run(),
    "18" => Example4.Run(),
    "19" => Example5.Run(),
    "20" => Example6.Run(),
    "21" => Example7.Run(),
    "22" => Example8.Run(),
    _ => 0
}}");
