using System.Windows.Markup;
using GtkDotNet;
using CsTools.Extensions;
using LinqTools;

// TODO StackSwitcher Stack(RefHandle<StackHandle>) 
// TODO RefHandle if zero call callback when handle is set
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
                        ))
                    .Child(
                        Box
                            .New(Orientation.Vertical)
                            .Append(
                                Stack.New()
                                .SideEffect(stack => 
                                    GetFiles()
                                        .ForEach(content => 
                                            stack.AddTitled(
                                                SrolledWindow
                                                    .New(),
                                                content.Name, content.Name)
                                            ))))
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
                file.GetBasename(), file.LoadStringContents() ?? ""));

    record FileContent(string Name, string Content);
}

