using static System.Console;

WriteLine(
    """
    Choose to run:
    1: First
    2: Hello World
    3: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    "2" => HelloWorld.Run(),
    _ => 0
}}");
