using GtkDotNet;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;
using CsTools.Functional;

static class Example2
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                .NewWindow()
                    .Title("Example Application 👍")
                    .DefaultSize(600, 400)
                    .Titlebar(
                        HeaderBar.New()
                        .TitleWidget(
                            StackSwitcher.New()
                            .StackRef(stack)
                        ))
                    .Child(
                        Box
                            .New(Orientation.Vertical)
                            .Append(
                                Stack.New()
                                .Ref(stack)
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
                                                .SetEditable(true)
                                                .SetCursorVisible(true)
                                                .Text(content.Content)),
                                            content.Name, content.Name)))))
                    .Show())
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
                file.GetBasename() ?? "", file.LoadStringContents() ?? ""));

    static readonly ObjectRef<StackHandle> stack = new();

    record FileContent(string Name, string Content);
}

