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
    8:  CSS
    9:  Progress
    10:  NotDecorated
    11:  Threading
    12: Cleanup
    13: Non GTK
    14: Example 1
    15: Example 2
    16: Example 3
    17: Example 4
    18: Example 5
    19: Example 6
    20: Example 7
    21: Example 8
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
    "8" => Css.Run(),
    "9" => Progress.Run(),
    "10"=> NotDecorated.Run(),
    "11"=> Threading.Run(),
    "12"=> Cleanup.Run(),
    "13"=> NonGtkApp.Run(),
    "14"=> Example1.Run(),
    "15" => Example2.Run(),
    "16" => Example3.Run(),
    "17" => Example4.Run(),
    "18" => Example5.Run(),
    "19" => Example6.Run(),
    "20" => Example7.Run(),
    "21" => Example8.Run(),
    _ => 0
}}");
