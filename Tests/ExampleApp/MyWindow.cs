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
        stack.OnNotify("visible-child", () => searchbar.SearchMode = false);

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
        search.Sensitive = true;
    }

    void ShowPreferences()
    {
        using var builder = Builder.FromDotNetResource("preferences");
        var prefs = new Preferences(this, builder, "preferences");
        prefs.Present();
    }

    void SearchTextChanged()
    {
        var text = searchEntry.AsEditable().GetText();
        var textview = stack.GetVisibleChild<ScrolledWindow>()?.GetChild<TextView>();
        var buffer = textview?.GetBuffer();
        if (textview == null || buffer == null)
            return;
        var startIter = buffer.GetStartIter();
        if (startIter.ForwardSearch(text, SearchFlags.CaseInsensitive) is var range && range.HasValue)
        {
            buffer.SelectRange(range.Value);
            textview.ScrollToIter(range.Value.Start);
        }
    }

    [Widget]
    Stack stack = null!;

    [Widget]
    Widget search = null!;

    [Widget]
    SearchEntry searchEntry = null!;

    [Widget]
    SearchBar searchbar = null!;
}

class MyButton : Button
{
    public MyButton(Builder builder, string? name = null) : base(builder, name) 
        => Console.WriteLine("My custom Button created");
}