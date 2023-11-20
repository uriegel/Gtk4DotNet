using static System.Console;

WriteLine(
    """
    Choose to run:
    1: First
    2: Hello World
    3: Packing buttons
    4: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    "2" => HelloWorld.Run(),
    "3" => PackingButtons.Run(),
    _ => 0
}}");
