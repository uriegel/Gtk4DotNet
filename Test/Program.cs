using static System.Console;

WriteLine(
    """
    Choose to run:
    1: First
    2: Hello World
    3: Packing buttons
    4: Drawing
    5: Builder
    6: Web View
    7: Threading
    8: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    "2" => HelloWorld.Run(),
    "3" => PackingButtons.Run(),
    "4" => Drawing.Run(),
    "5" => BuilderProgram.Run(),
    "6" => Web.Run(),
    "7" => Threading.Run(),
    _ => 0
}}");
