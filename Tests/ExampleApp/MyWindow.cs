using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public static MyWindow? Instance { get; private set; }
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        Instance = this;

        using var settings = GSettings.New(Globals.ApplicationId);
        settings.Bind("transition", stack, "transition-type", BindFlags.Default);

        AddActions(
            new SimpleAction("preferences", ShowPreferences),
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q")
        );

    }

    public void OnOpen(GFile file)
    {
        using var builder = Builder.FromDotNetResource("fileview");
        using var fileView = new FileView(file.LoadStringContents(), builder, "fileview");
        stack.AddTitled(fileView, file.GetBasename(), file.GetBasename());
    }

    void ShowPreferences()
    {
        using var builder = Builder.FromDotNetResource("preferences");
        var prefs = new Preferences(this, builder, "preferences");
        prefs.Present();
    }

    [Widget]
    Stack stack = null!;
}

class MyButton : Button
{
    public MyButton(Builder builder, string? name = null) : base(builder, name) 
        => Console.WriteLine("My custom Button created");
}