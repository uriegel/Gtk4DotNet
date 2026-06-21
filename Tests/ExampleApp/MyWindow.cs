using System.ComponentModel;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public static MyWindow? Instance { get; private set; }
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        Instance = this;
    }

    public void OnOpen(GFile file)
    {
        using var builder = Builder.FromDotNetResource("fileview");
        using var fileView = new FileView(file.LoadStringContents(), builder, "fileview");
        stack.AddTitled(fileView, file.GetBasename(), file.GetBasename());
    }

    [Widget]
    Stack stack = null!;
}

class MyButton : Button
{
    public MyButton(Builder builder, string? name = null) : base(builder, name) 
        => Console.WriteLine("My custom Button created");
}