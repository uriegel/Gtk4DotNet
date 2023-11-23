using GtkDotNet;
using CsTools.Extensions;
using LinqTools;
using GtkDotNet.SafeHandles;

static class Example4
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
                                    .AppendItem(MenuItem.New("_Quit", "app.quit")))))
                        ))
                    .Child(
                        Box
                            .New(Orientation.Vertical)
                            .Append(
                                Stack.New()
                                .Ref(stack)
                                .SideEffect(s => Settings.Bind(settings, "transition", s, "transition-type", BindFlags.Default))
                                .SideEffect(stack => 
                                    GetFiles()
                                        .ForEach(content => 
                                            stack.AddTitled(
                                                ScrolledWindow
                                                    .New()
                                                    .HExpand(true)
                                                    .VExpand(true)
                                                    .Child(
                                                        TextView.New()
                                                        .SetEditable(false)
                                                        .SetCursorVisible(true)
                                                        .Text(content.Content)
                                                        .SideEffect(t =>
                                                        {
                                                            var tag = t.GetBuffer().CreateTag();
                                                            Settings.Bind(settings, "font", tag, "font", BindFlags.Default);
                                                        })),
                                                content.Name, content.Name)
                                            ))))
                        .Show())
            .AddActions(new GtkAction[]
            {
                new("preferences", () => Dialog4.PreferenceDialog.Show(window.Ref, settings)),
                new("quit", () => window.Ref.CloseWindow(), "<Ctrl>Q")
            })
            .Run(0, IntPtr.Zero);

    static IEnumerable<FileContent> GetFiles()
        => new[] {
            "First.cs",
            "Drawing.cs",
            "Example2.cs"
        }.Select(GetFile);

    static FileContent GetFile(string path)
        => GFile.New(Directory.GetCurrentDirectory().AppendPath(path)).Use(
            file => new FileContent(
                file.GetBasename(), file.LoadStringContents() ?? ""));

    static SettingsHandle settings = new();
    static readonly WidgetRef<WindowHandle> window = new();
    static readonly WidgetRef<StackHandle> stack = new();

    record FileContent(string Name, string Content);
}

