using static System.Console;

WriteLine(
    """
    Choose to run:
    1: First
    2: Quit
    """);
WriteLine($"Return value: {ReadLine() switch 
{
    "1" => First.Run(),
    _ => 0
}}");
