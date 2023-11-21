using static System.Console;

WriteLine(
    """
    Choose to run:
    1: First
    2: Hello World
    3: Packing buttons
    4: Builder
    5: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    "2" => HelloWorld.Run(),
    "3" => PackingButtons.Run(),
    "4" => BuilderProgram.Run(),
    _ => 0
}}");
