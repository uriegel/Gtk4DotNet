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
    8:  Progress
    0:  NotDecorated
    10:  Threading
    11: Cleanup
    12: Non GTK
    13: Example 1
    14: Example 2
    15: Example 3
    16: Example 4
    17: Example 5
    18: Example 6
    19: Example 7
    20: Example 8
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
    "8" => Progress.Run(),
    "9" => NotDecorated.Run(),
    "10"=> Threading.Run(),
    "11"=> Cleanup.Run(),
    "12"=> NonGtkApp.Run(),
    "13"=> Example1.Run(),
    "14" => Example2.Run(),
    "15" => Example3.Run(),
    "16" => Example4.Run(),
    "17" => Example5.Run(),
    "18" => Example6.Run(),
    "19" => Example7.Run(),
    "20" => Example8.Run(),
    _ => 0
}}");
