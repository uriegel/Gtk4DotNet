using CsTools.Extensions;
using Gtk4DotNet;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    using var window = app.NewWindow();
    window.Title = "With Style👍";
    window.SetDefaultSize(200, 200);
    StyleContext.AddProviderForDisplay(
        Display.GetDefault(),
        CssProvider.New().FromResource("style"),
        StyleProviderPriority.Application);
    window.SetChild(new Box(Orientation.Vertical, 10)
        .Margin(10)
        .Append(new Button("Button 1"))
        .Append(new Button("Button 2").CssClass("button-1"))
        .Append(new Button("Hover me!").SetName("button-2"))
        .Append(new MenuButton())
        .Append(new Button("Suggested").CssClass("destructive-action"))
        .Append(new Button("Destructive").CssClass("suggested-action")));
    window.Show();
};
app.Run();


