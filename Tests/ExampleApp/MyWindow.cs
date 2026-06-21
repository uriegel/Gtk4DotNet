using System.ComponentModel;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public static MyWindow? Instance { get; private set; }
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        Instance = this;
    }
}

class MyButton : Button
{
    public MyButton(Builder builder, string? name = null) : base(builder, name) 
        => Console.WriteLine("My custom Button created");
}