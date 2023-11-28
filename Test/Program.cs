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
    8:  Threading
    9:  Cleanup
    10: Non GTK
    11: Example 1
    12: Example 2
    13: Example 3
    14: Example 4
    15: Example 5
    16: Example 6
    17: Example 7
    18: Example 8
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
    "8" => Threading.Run(),
    "9" => Cleanup.Run(),
    "10" => NonGtkApp.Run(),
    "11"=> Example1.Run(),
    "12" => Example2.Run(),
    "13" => Example3.Run(),
    "14" => Example4.Run(),
    "15" => Example5.Run(),
    "16" => Example6.Run(),
    "17" => Example7.Run(),
    "18" => Example8.Run(),
    _ => 0
}}");
