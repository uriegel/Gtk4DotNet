using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public static MyWindow? Instance { get; private set; }
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        Instance = this;
        sidebarRevealer.IsRevealed = true;

        Application.Settings.Bind("transition", stack, "transition-type");
        Application.Settings.Bind("show-words", sidebarRevealer, "reveal-child");
        searchEntry.OnSearchChanged += SearchTextChanged;
        search.BindProperty("active", searchbar, "search-mode-enabled", BindingFlags.Bidirectional);
        lines.BindProperty("visible", linesLabel, "visible");
        stack["visible-child"].OnNotify += () =>
        {
            searchbar.SearchMode = false;
            UpdateWords();
            UpdateLines();
        };
        sidebarRevealer["reveal-child"].OnNotify += UpdateWords;
        AddActions(
            new SimpleAction("preferences", ShowPreferences),
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q"),
            Application.Settings.CreateAction("show-words", "<Ctrl>W"),
            lines.CreatePropertyAction("show-lines", "visible", "<Ctrl>L")
        );
    }

    public void OnOpen(GFile file)
    {
        using var builder = Builder.FromDotNetResource("fileview");
        using var fileView = new FileView(file.LoadStringContents(), builder, "fileview");
        stack.AddTitled(fileView, file.GetBasename() ?? "", file.GetBasename() ?? "");
        search.Sensitive = true;
        UpdateWords();
        UpdateLines();
    }

    void ShowPreferences()
    {
        using var builder = Builder.FromDotNetResource("preferences");
        var prefs = new Preferences(this, builder, "preferences");
        prefs.Present();
    }

    void SearchTextChanged()
    {
        var text = searchEntry.AsEditable().Text;
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

    void UpdateLines()
    {
        var textview = stack.GetVisibleChild<ScrolledWindow>()?.GetChild<TextView>();
        var buffer = textview?.GetBuffer();
        if (buffer == null)
            return;
        lines.Text = $"{buffer.LineCount}";       
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
    readonly Stack stack = null!;

    [Widget]
    readonly Widget search = null!;

    [Widget]
    readonly SearchEntry searchEntry = null!;

    [Widget]
    readonly SearchBar searchbar = null!;

    [Widget]
    readonly Revealer sidebarRevealer = null!;

    [Widget]
    readonly ListBox words = null!;

    [Widget]
    readonly Label lines = null!;

    [Widget]
    readonly Label linesLabel = null!;
}

class MyButton : Button
{
    public MyButton(Builder builder, string? name = null) : base(builder, name) 
        => Console.WriteLine("My custom Button created");
}