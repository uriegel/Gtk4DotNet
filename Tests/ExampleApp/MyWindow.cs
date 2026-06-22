using Gtk4DotNet;

// TODO leaking when selecting via search, i a simple C program too!

class MyWindow : ApplicationWindow
{
    public static MyWindow? Instance { get; private set; }
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        Instance = this;
        sidebarRevealer.IsRevealed = true;

        using var settings = GSettings.New(Globals.ApplicationId);
        settings.Bind("transition", stack, "transition-type", BindFlags.Default);
        settings.Bind("show-words", sidebarRevealer, "reveal-child", BindFlags.Default);
        searchEntry.OnSearchChanged(SearchTextChanged);
        search.BindProperty("active", searchbar, "search-mode-enabled", BindingFlags.Bidirectional);
        stack.OnNotify("visible-child", () =>
        {
            searchbar.SearchMode = false;
            UpdateWords();
        });
        sidebarRevealer.OnNotify("reveal-child", UpdateWords);
        AddActions(
            new SimpleAction("preferences", ShowPreferences),
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q"),
                settings.CreateAction("show-words", "<Ctrl>W")
        );
    }

    public void OnOpen(GFile file)
    {
        using var builder = Builder.FromDotNetResource("fileview");
        using var fileView = new FileView(file.LoadStringContents(), builder, "fileview");
        stack.AddTitled(fileView, file.GetBasename(), file.GetBasename());
        search.Sensitive = true;
        UpdateWords();
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
        using var buffer = textview?.GetBuffer();
        if (textview == null || buffer == null)
            return;
        var startIter = buffer.GetStartIter();
        if (startIter.ForwardSearch(text, SearchFlags.CaseInsensitive) is var range && range.HasValue)
        {
            buffer.SelectRange(range.Value);
            textview.ScrollToIter(range.Value.Start);
        }
    }

    void UpdateWords()
    {
        var textview = stack.GetVisibleChild<ScrolledWindow>()?.GetChild<TextView>();
        var buffer = textview?.GetBuffer();
        if (textview == null || buffer == null)
            return;

        var wordHash = GetWords(buffer).ToHashSet();
        words.RemoveAll();
        foreach (var word in wordHash)
        {
            var item = Label.New(word);
            words.Append(item);
        }
    }

    static IEnumerable<string> GetWords(TextBuffer buffer)
    {

        var start = buffer.GetStartIter();
        var end = new TextIter();
        while (!start.IsEnd())
        {
            while (!start.StartsWord())
            {
                if (!start.ForwardChar())
                    yield break;
            }
            end = start;
            if (!end.ForwardWordEnd())
                yield break;
            yield return buffer.GetText(start, end, false);
            start = end;
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

    [Widget]
    Revealer sidebarRevealer = null!;

    [Widget]
    ListBox words = null!;
}

class MyButton : Button
{
    public MyButton(Builder builder, string? name = null) : base(builder, name) 
        => Console.WriteLine("My custom Button created");
}