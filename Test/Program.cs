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
    10:  Example 1
    11: Example 2
    12: Example 3
    13: Example 4
    14: Example 5
    15: Example 6
    16: Example 7
    17: Non GTK
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
    "10"=> Example1.Run(),
    "11" => Example2.Run(),
    "12" => Example3.Run(),
    "13" => Example4.Run(),
    "14" => Example5.Run(),
    "15" => Example6.Run(),
    "16" => Example7.Run(),
    "17" => NonGtkApp.Run(),
    _ => 0
}}");
