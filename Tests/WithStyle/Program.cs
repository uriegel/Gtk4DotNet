using CsTools.Extensions;
using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("With Style👍")
        .DefaultSize(200, 200)
        .SideEffect(_ => StyleContext.AddProviderForDisplay(
            Display.GetDefault(),
            CssProvider.New().FromResource("style"),
            StyleProviderPriority.Application))
        .Child(Box
            .New(Orientation.Vertical, 10)
            .Margin(10)
            .Append(Button.NewWithLabel("Button 1"))
            .Append(Button.NewWithLabel("Button 2").CssClass("button-1"))
            .Append(Button.NewWithLabel("Hover me!").SetName("button-2"))
            .Append(MenuButton.New())
            .Append(Button.NewWithLabel("Suggested").CssClass("destructive-action"))
            .Append(Button.NewWithLabel("Destructive").CssClass("suggested-action")))
        .Show()
    ).Run();


