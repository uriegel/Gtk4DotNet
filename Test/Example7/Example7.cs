using GtkDotNet;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;
using CsTools.Functional;

static class Example7
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .SideEffect(a => settings = Settings.New("org.gtk.exampleapp"))
            .OnActivate(app =>
                app
                .NewWindow()
                    .Ref(window)
                    .Title("Example Application 👍")
                    .DefaultSize(600, 400)
                    .Titlebar(
                        HeaderBar.New()
                        .SideEffect(hb => 
                            Label.New("Lines:")
                            .SideEffect(label => hb
                                .PackStart(
                                    label
                                    .Visible(false))
                                .PackStart(
                                    Label.New("")
                                    .Ref(lines)
                                    .BindProperty("visible", label, "visible", BindingFlags.Default)
                                    .Visible(false))))
                        .TitleWidget(
                            StackSwitcher.New()
                            .StackRef(stack))
                        .PackEnd(
                            MenuButton.New()
                            .Direction(Arrow.None)
                            .Model(Menu.New()
                                .AppendItem(MenuItem.NewSection(null,
                                    Menu.New()
                                    .AppendItem(MenuItem.New("_Words", "win.show-words"))
                                    .AppendItem(MenuItem.New("_Lines", "win.show-lines"))
                                    .AppendItem(MenuItem.New("_Preferences", "app.preferences"))))
                                .AppendItem(MenuItem.NewSection(null,
                                    Menu.New()
                                    .AppendItem(MenuItem.New("_Quit", "app.quit"))))))
                        .PackEnd(
                            ToggleButton.New()
                            .Ref(search)
                            .BindProperty("active", searchBar, "search-mode-enabled", BindingFlags.Bidirectional)
                            .IconName("edit-find-symbolic")
                            .Sensitive(false))
                        )
                    .Child(
                        Box
                            .New(Orientation.Vertical)
                            .Append(
                                SearchBar.New()
                                .Ref(searchBar)
                                .Child(
                                    SearchEntry.New()
                                    .Ref(searchEntry)
                                    .OnSearchChanged(SearchTextChanged)))
                            .Append(
                                Box.New(Orientation.Horizontal)
                                .Append(
                                    Revealer.New()
                                    .SideEffect(r => Settings.Bind(settings, "show-words", r, "reveal-child", BindFlags.Default))
                                    .TransitionType(RevealerTransition.SwingRight)
                                    .OnNotify("reveal-child", _ => UpdateWords())
                                    .Child(
                                        ScrolledWindow
                                            .New()
                                            .Policy(PolicyType.Never, PolicyType.Automatic)
                                            .Child(
                                                ListBox.New()
                                                .Ref(wordsBox)
                                                .SelectionMode(SelectionMode.None))))
                                .Append(
                                    Stack.New()
                                    .OnNotify("visible-child", OnStackChanged)
                                    .Ref(stack)
                                    .SideEffect(s => Settings.Bind(settings, "transition", s, "transition-type", BindFlags.Default))
                                    .SideEffect(stack =>
                                        GetFiles()
                                            .SideEffect(files => search.Ref.Sensitive(files.Length > 0))
                                            .ForEach(content =>
                                                stack.AddTitled(
                                                    ScrolledWindow
                                                        .New()
                                                        .HExpand(true)
                                                        .VExpand(true)
                                                        .Child(
                                                            TextView.New()
                                                            .Name("TextView")
                                                            .SetEditable(false)
                                                            .SetCursorVisible(true)
                                                            .Text(content.Content)
                                                            .SideEffect(t =>
                                                            {
                                                                var buffer = t.GetBuffer();
                                                                var tag = buffer.CreateTag();
                                                                Settings.Bind(settings, "font", tag, "font", BindFlags.Default);
                                                                buffer.ApplyTag(tag, buffer.GetStartIter(), buffer.GetEndIter());
                                                            })),
                                                    content.Name, content.Name)
                                                )))))
                    .AddAction(settings.CreateAction("show-words"))
                    .AddAction(PropertyAction.New("show-lines", lines.Ref, "visible"))
                    .SideEffect(_ => UpdateWords())
                    .SideEffect(_ => UpdateLines())
                    .Show())
            .AddActions(new GtkAction[]
            {
                new("preferences", () => new Dialog7.PreferenceDialog().Show(window.Ref, settings)),
                new("quit", () => window.Ref.CloseWindow(), "<Ctrl>Q")
            })
            .Run(0, IntPtr.Zero);

    static FileContent[] GetFiles()
        => new []
            {
                "First.cs",
                "Drawing.cs",
                "Example2.cs"
            }
            .Select(GetFile)
            .ToArray();

    static FileContent GetFile(string path)
        => GFile.New(Directory.GetCurrentDirectory().AppendPath(path)).Use(
            file => new FileContent(
                file.GetBasename() ?? "", file.LoadStringContents() ?? ""));

    static void SearchTextChanged(SearchEntryHandle entry)
        => stack.Ref
            .GetVisibleChild()
            .FindWidget(n => n.GetName() == "TextView")
            ?.DownCastTextViewHandle()
            ?.SideEffect(textView =>
            {
                var buffer = textView.GetBuffer();
                var result = buffer.GetStartIter().ForwardSearch(entry.GetText(), SearchFlags.CaseInsensitive);
                if (result.HasValue)
                {
                    var range = buffer.SelectRange(result.Value);
                    textView.ScrollToIter(range.Start);
                }
            });

    static void UpdateWords() 
        => stack.Ref
            .GetVisibleChild()
            .FindWidget(n => n.GetName() == "TextView")
            ?.DownCastTextViewHandle()
            ?.SideEffect(_ => wordsBox.Ref.RemoveAll())
            ?.GetText()
                .Split(new[] { ' ', '\n', '.', '"', '(', ')', ';', '}', '{', '/', ',', '<', '>', '=' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .Where(n => n.Length > 0)
                .Distinct()
                .ForEach(n => 
                    Button
                    .NewWithLabel(n)
                    .SideEffect(b => b.OnClicked(() =>
                        searchEntry.Ref.SetText(b.GetLabel())))
                    .SideEffect(b => wordsBox.Ref.Insert(b)));

    static void OnStackChanged(StackHandle _) 
    {
        searchBar.Ref.SearchMode(false);
        UpdateWords();
        UpdateLines();
    } 

    static void UpdateLines()
        => stack.Ref
            .GetVisibleChild()
            .FindWidget(n => n.GetName() == "TextView")
            ?.DownCastTextViewHandle()
            ?.SideEffect(tv => lines.Ref.Set($"{tv.GetText().Length}"));

    static SettingsHandle settings = new();
    static readonly ObjectRef<WindowHandle> window = new();
    static readonly ObjectRef<StackHandle> stack = new();
    static readonly ObjectRef<ToggleButtonHandle> search = new();
    static readonly ObjectRef<SearchBarHandle> searchBar = new();
    static readonly ObjectRef<ListBoxHandle> wordsBox = new();
    static readonly ObjectRef<SearchEntryHandle> searchEntry = new();
    static readonly ObjectRef<LabelHandle> lines = new();
    
        
    record FileContent(string Name, string Content);
}

