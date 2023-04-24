using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var settings = Settings.New("org.gtk.exampleapp");

    Application.RegisterResources();
    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    var stack = Builder.GetObject(builder, "stack");
    var gears = Builder.GetObject(builder, "gears");
    var search = Builder.GetObject(builder, "search");
    var searchBar = Builder.GetObject(builder, "searchbar");
    var searchEntry = Builder.GetObject(builder, "searchentry");
    Window.SetApplication(window, app);
    GObject.Unref(builder);

    var menuBuilder = Builder.FromResource("/org/gtk/example/menu.ui");
    var menu = Builder.GetObject(menuBuilder, "menu");
    MenuButton.SetModel(gears, menu);
    GObject.Unref(menuBuilder);

    GObject.BindProperty(search, "active", searchBar, "search-mode-enabled", BindingFlags.Bidirectional);

    Settings.Bind(settings, "transition", stack, "transition-type", BindFlags.Default);
    var openPreferences = () =>
    {
        var dialogBuilder = Builder.FromResource("/org/gtk/example/dialog.ui");
        var dialog = Builder.GetObject(dialogBuilder, "dialog");
        var transition = Builder.GetObject(dialogBuilder, "transition");
        var font = Builder.GetObject(dialogBuilder, "font");

        Window.SetTransientFor(dialog, window);
        Widget.Show(dialog);
        GObject.Unref(dialogBuilder);
        Settings.Bind(settings, "font", font, "font", BindFlags.Default);
        Settings.Bind(settings, "transition", transition, "active-id", BindFlags.Default);
    };

    var actions = new GtkAction[] 
    {
        new GtkAction("preferences", openPreferences),
        new GtkAction("show-words", () => Window.Close(window), "<Ctrl>Q"),
        new GtkAction("quit", () => Window.Close(window), "<Ctrl>Q")
    };
    Application.AddActions(app, actions);

    void searchTextChanged(IntPtr z1, IntPtr z2)
    {
        var text = searchEntry.EditableGetText();
        if (text.Length == 0)
            return;

        var tab = Stack.GetVisibleChild(stack);
        var textView = ScrolledWindow.GetChild(tab);
        var buffer = TextView.GetBuffer(textView);
        
        TextBuffer.GetStartIter(buffer, out var startIter);
        if (TextIter.ForwardSearch(ref startIter, text, SearchFlags.CaseInsensitive, out var matchStart, out var matchEnd, IntPtr.Zero))
        {
            TextBuffer.SelectRange(buffer, ref matchStart, ref matchEnd);
            TextView.ScrollToIter(textView, ref matchStart, 0.0, false, 0.0, 0.0);
        }
    }

    void visibleChildChanged(IntPtr stack, IntPtr pec,  IntPtr z)
        => SearchBar.SetSearchMode(searchBar, false);

    Gtk.SignalConnect<TwoIntPtr>(searchEntry, "search-changed", searchTextChanged);
    Gtk.SignalConnect<ThreeIntPtr>(stack, "notify::visible-child", visibleChildChanged);
    Gtk.SignalConnect(window, "destroy", () => GObject.Clear(settings));

    Widget.Show(window);

    var currentDirectory = Directory.GetCurrentDirectory();
    var files = System.Environment.CommandLine.Split(' ').Skip(1).Select(n => GFile.New(Path.Combine(currentDirectory, n)));
    foreach (var file in files)
    {
        var name = GFile.GetBasename(file);
        var scrolled = ScrolledWindow.New ();
        Widget.SetHExpand(scrolled, true);
        Widget.SetVExpand(scrolled, true);
        var textView = TextView.New();
        TextView.SetEditable(textView, false);
        TextView.SetCursorVisible(textView, false);
        ScrolledWindow.SetChild(scrolled, textView);
        Stack.AddTitled(stack, scrolled, name, name);
        var content = GFile.LoadContents(file);
        if (content.HasValue)
        {
            var buffer = TextView.GetBuffer(textView);
            TextBuffer.SetText(buffer, content.Value.content, (int)content.Value.length);
            GObject.Free(content.Value.content);

            var tag = TextBuffer.CreateTag(buffer);
            Settings.Bind(settings, "font", tag, "font", BindFlags.Default);

            TextBuffer.GetStartIter(buffer, out var startIter);
            TextBuffer.GetEndIter(buffer, out var endIter);
            TextBuffer.ApplyTag(buffer, tag, ref startIter, ref endIter);
        }
    }
    Widget.SetSensitive(search, true);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;
