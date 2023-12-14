using GtkDotNet;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;
using CsTools.Functional;

static class Example5
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
                        .TitleWidget(
                            StackSwitcher.New()
                            .StackRef(stack))
                        .PackEnd(
                            MenuButton.New()
                            .Direction(Arrow.None)
                            .Model(Menu.New()
                                .AppendItem(MenuItem.NewSection(null,
                                    Menu.New()
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
                                    .OnSearchChanged(SearchTextChanged)))
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
                                            ))))
                        .Show())
            .AddActions(new GtkAction[]
            {
                new("preferences", () => new Dialog5.PreferenceDialog().Show(window.Ref, settings)),
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
    {
        var textView =
            stack.Ref
                .GetVisibleChild()
                .FindWidget(n => n.GetName() == "TextView")
                ?.DownCastTextViewHandle();
        if (textView != null)
        {
            var buffer = textView.GetBuffer();
            var startIter = buffer.GetStartIter();
            var result = startIter.ForwardSearch(entry.GetText() ?? "", SearchFlags.CaseInsensitive);
            if (result.HasValue)
            {
                var range = buffer.SelectRange(result.Value);
                textView.ScrollToIter(range.Start);
            }
        }
    }

    static void OnStackChanged(StackHandle _) => searchBar.Ref.SearchMode(false);

    static SettingsHandle settings = new();
    static readonly ObjectRef<WindowHandle> window = new();
    static readonly ObjectRef<StackHandle> stack = new();
    static readonly ObjectRef<ToggleButtonHandle> search = new();
    static readonly ObjectRef<SearchBarHandle> searchBar = new();
        
    record FileContent(string Name, string Content);
}

