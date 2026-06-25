using CsTools.Extensions;
using Gtk4DotNet;

using static System.Console;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("Pack👍")
        .Pipe(win => win.Child(
            Grid
                .New()
                .Attach(
                    Button
                        .NewWithLabel("Button 1")
                        .SideEffect(b => b.OnClicked += () => WriteLine("Button1 clicked")), 0, 0, 1, 1)
                .Attach(
                    Button
                        .NewWithLabel("Button 2")
                        .SideEffect(b => b.OnClicked += () => WriteLine("Button2 clicked")), 1, 0, 1, 1)
                .Attach(
                    Button
                        .NewWithLabel("Quit")
                        //.Clicked(() => win.CloseWindow()), 0, 1, 2, 1)))
                        .SideEffect(b => b.OnClicked += ShowWindow), 0, 1, 2, 1)))
        .Show()
    ).Run();

void ShowWindow()
    => new MyWindow()
        .Title("Child👍")
        .Show();

class MyWindow : Window
{
    public MyWindow()
    {
        Construct();
        Button button = Button.NewWithLabel("Test");
        button.OnClicked += Klicḱen;
        SetChild(button);

        OnFinalize(() => button.OnClicked -= Klicḱen);
    }

    void Klicḱen() => WriteLine("Geklickt");
};