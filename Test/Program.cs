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
    8:  NotDecorated
    9:  Threading
    10: Cleanup
    11: Non GTK
    12: Example 1
    13: Example 2
    14: Example 3
    15: Example 4
    16: Example 5
    17: Example 6
    18: Example 7
    19: Example 8
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
    "8" => NotDecorated.Run(),
    "9" => Threading.Run(),
    "10"=> Cleanup.Run(),
    "11"=> NonGtkApp.Run(),
    "12"=> Example1.Run(),
    "13" => Example2.Run(),
    "14" => Example3.Run(),
    "15" => Example4.Run(),
    "16" => Example5.Run(),
    "17" => Example6.Run(),
    "18" => Example7.Run(),
    "19" => Example8.Run(),
    _ => 0
}}");
