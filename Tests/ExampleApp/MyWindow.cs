using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public static MyWindow? Instance { get; private set; }
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        Instance = this;

        using var settings = GSettings.New(Globals.ApplicationId);
        settings.Bind("transition", stack, "transition-type", BindFlags.Default);
        searchEntry.OnSearchChanged(SearchTextChanged);
        search.BindProperty("active", searchbar, "search-mode-enabled", BindingFlags.Bidirectional);

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
        // TODO search.Sensitive = true
    }

    void ShowPreferences()
    {
        using var builder = Builder.FromDotNetResource("preferences");
        var prefs = new Preferences(this, builder, "preferences");
        prefs.Present();
    }

    void SearchTextChanged()
    {
        var text = (searchEntry as Editable).GetText();
    }

    [Widget]
    Stack stack = null!;

    [Widget]
    Widget search = null!;

    [Widget]
    SearchEntry searchEntry = null!;

    [Widget]
    Widget searchbar = null!;
}

class MyButton : Button
{
    public MyButton(Builder builder, string? name = null) : base(builder, name) 
        => Console.WriteLine("My custom Button created");
}