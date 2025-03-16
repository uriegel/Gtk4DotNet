using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

using static System.Console;

static class SubClassing
{
    public static int Run()
    {
        WriteLine("1 - Custom Buttom");
        WriteLine("2 - Custom Window");
        WriteLine("3 - Custom Window with WebView");        

        var input = ReadLine();
        switch (input)
        {
            case "1":
                RunButton();
                break;
            case "2":
                RunCustomWindow();
                break;
            case "3":
                RunCustomWindowWithWebView();
                break;
        }
        return 0;
    }
    static void RunButton()
        => Application
           .New("org.gtk.example")
           .OnActivate(app =>
               app
                   .SubClass(new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                   .NewWindow()
                       .Title("Hello Gtk👍")
                       .DefaultSize(600, 200)
                       .Child(
                            Box
                                .New(Orientation.Vertical)
                                .Append(CustomButton.Create("CustomButton")
                                    .Label("Button 1")
                                    .OnSlowClicked(p => WriteLine($"slow click event received: {p}")))
                                .Append(CustomButton.Create("CustomButton")
                                    .Label("Button 2"))
                       )
                       .Show())
            .Run(0, IntPtr.Zero);

    static void RunCustomWindow()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => app
                .SubClass(new CustomWindowClass(GTypeEnum.ApplicationWindow, "CustomWindow", p => new CustomWindow(p)))
                .SubClass(new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                .CustomWindow("CustomWindow")
                    .Pipe(win => win
                        .AddActions(
                            [
                                new("custom-action", () => WriteLine("Custom Action activated"), "F2"),
                                new("quit", () => win.CloseWindow(), "<Ctrl>Q")
                            ]))
                    .Show())
            .Run(0, IntPtr.Zero);

    static void RunCustomWindowWithWebView()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => app
                .SubClass(new CustomWindowWithWebViewClass(GTypeEnum.ApplicationWindow, "CustomWindowWithWebView", p => new CustomWindowWithWebView(p)))
                .CustomWindow("CustomWindowWithWebView")
                    .Pipe(win => win
                        .AddActions(
                            [ new("quit", () => win.CloseWindow(), "<Ctrl>Q") ]))
                    .Show())
            .Run(0, IntPtr.Zero);
}


// Custom Button ========================================================================================================================

class CustomButtonClass(GTypeEnum parent, string name, Func<nint, CustomButton> constructor)
    : SubClass<ButtonHandle>(parent, name, constructor)
{
    public static uint SlowClick;

    public const int PROP_TESTTITLE = 1;

    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        RegisterProperty(cls, PROP_TESTTITLE, "testtitle");
        SlowClick = NewSignal(Type, "slow_click", SignalFlags.RunLast, GTypes.None, [ GTypes.String ]);
    }
}

class CustomButton(nint obj) : SubClassInst<ButtonHandle>(obj)
{
    protected override void OnCreate() =>
        Handle.OnClicked(async () =>
        {
            WriteLine($"testtitle: {testTitle}");
            Handle.Label($"{++count} times clicked");
            testTitle = Handle.GetLabel();
            Handle.Notify("testtitle");

            IntPtr args = Marshal.StringToHGlobalAuto("Slow click emitted");
            await Task.Delay(1000);
            Handle.EmitSignal(CustomButtonClass.SlowClick, 0, args);
            Marshal.FreeHGlobal(args);
        });

    protected override void OnFinalize() => WriteLine("Button finalized");
    protected override ButtonHandle CreateHandle(nint obj) => new(obj);

    protected override void OnSetProperty(uint propId, nint value)
    {
        if (propId == CustomButtonClass.PROP_TESTTITLE)
            testTitle = GValue.GetString(value);
    }

    protected override void OnGetProperty(uint propId, nint value)
    {
        if (propId == CustomButtonClass.PROP_TESTTITLE)
            GValue.SetString(value, testTitle);
    }

    string? testTitle;

    int count;
}

static class CustomButtonExtensions
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void SlowClickDelegate(nint _, string p, nint __);
    public static ButtonHandle OnSlowClicked(this ButtonHandle handle, Action<string> click)
    {
        Gtk.SignalConnect<SlowClickDelegate>(handle, "slow_click", (_, p, __) => click(p));
        return handle;
    }
}

// Custom Window ========================================================================================================================

class CustomWindowClass(GTypeEnum parent, string name, Func<nint, CustomWindow> constructor)
    : SubClass<ApplicationWindowHandle>(parent, name, constructor)
{
    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, "windowsubclass");
    }
}

class CustomWindow(nint obj) : SubClassInst<ApplicationWindowHandle>(obj)
{
    protected override void OnCreate()
    {
        Handle.InitTemplate();
        Handle
            .GetTemplateChild<ButtonHandle, ApplicationWindowHandle>("button1")
            ?.OnClicked(() => WriteLine("Button1 clicked"));
        Handle
            .GetTemplateChild<ButtonHandle, ApplicationWindowHandle>("quit")
            ?.OnClicked(() => Handle.CloseWindow());
    }
    protected override void OnFinalize() => WriteLine("Window finalized");
    protected override ApplicationWindowHandle CreateHandle(nint obj) => new(obj);
}

// Custom Window with WebView========================================================================================================================

class CustomWindowWithWebViewClass(GTypeEnum parent, string name, Func<nint, CustomWindowWithWebView> constructor)
    : SubClass<ApplicationWindowHandle>(parent, name, constructor)
{
    protected override void ClassInit(nint cls, nint _)
    {
        var webkitType = GType.Get(GTypeEnum.WebKitWebView);
        GType.Ensure(webkitType);
        var type = "WebKitWebView".TypeFromName();
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, "windowwebviewsubclass");
    }
}

class CustomWindowWithWebView(nint obj) : SubClassInst<ApplicationWindowHandle>(obj)
{
    protected override void OnCreate()
    {
        Handle.InitTemplate();
        Handle  
            .GetTemplateChild<WebViewHandle, ApplicationWindowHandle>("webview")
            ?.LoadUri("https://github.com/uriegel/Gtk4DotNet");
    }
    protected override void OnFinalize() => WriteLine("Window finalized");
    protected override ApplicationWindowHandle CreateHandle(nint obj) => new(obj);
}

