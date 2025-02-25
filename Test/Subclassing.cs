using System.Runtime.InteropServices;
using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

using static System.Console;

static class SubClassing
{
    public static int Run()
    {
        WriteLine("1 - GObject");
        WriteLine("2 - Custom Buttom");
        WriteLine("3 - Custom Buttom in template");
        WriteLine("4 - Custom Window");
        WriteLine("5 - Custom Window with WebView");        

        var input = ReadLine();
        switch (input)
        {
            case "1":
                RunGObject();
                break;
            case "2":
                RunButton();
                break;
            case "3":
                RunBuilder();
                break;
            case "4":
                RunCustomWindow();
                break;
            case "5":
                RunCustomWindowWithWebView();
                break;
        }
        return 0;
    }
    static void RunGObject()
    {
        var typeDouble = "TDouble".TypeFromName();
        var tDoubleClass = new TDoubleClass(GTypeEnum.GObject, "TDouble", p => new TDouble(p));
        var customButtonClass = new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p));
        typeDouble = "TDouble".TypeFromName();
        var typeCustomButton = "CustomButton".TypeFromName();

        var obj = GObject.New<GObjectHandle>(typeDouble);
        var obj2 = GObject.New<GObjectHandle>(typeDouble);
        var refcount = Marshal.ReadInt32(obj.GetInternalHandle(), IntPtr.Size);
        obj2.Dispose();
        obj.Dispose();
        refcount = Marshal.ReadInt32(obj.GetInternalHandle(), IntPtr.Size);
    }

    static void RunButton()
        => Application
           .New("org.gtk.example")
           .OnActivate(app =>
               app
                   .SubClass(new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                   .SubClass(new TDoubleClass(GTypeEnum.Button, "TDouble", p => new TDouble(p)))
                   .NewWindow()
                       .Title("Hello Gtk👍")
                       .DefaultSize(600, 200)
                       .Child(
                            Box
                                .New(Orientation.Vertical)
                                .Append(GObject.New<ButtonHandle>("CustomButton".TypeFromName())
                                    .Label("Button 1"))
                                .Append(GObject.New<ButtonHandle>("CustomButton".TypeFromName())
                                    .Label("Button 2"))
                       )
                       .Show())
            .Run(0, IntPtr.Zero);

    public static int RunBuilder()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => app
                .SubClass(new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                .SideEffect(app =>
                    Builder.FromDotNetResource("buildersubclass").Use(
                        builder => builder
                            .GetObject<WindowHandle>("window", w => w
                                .SetApplication(app)
                                .SideEffect(w =>
                                    builder
                                        .SideEffect(b => b.GetObject<ButtonHandle>("button1", b => b
                                            .OnClicked(() => WriteLine("Button1 clicked"))))
                                        .SideEffect(b => b.GetObject<ButtonHandle>("button2", b => b
                                            .OnClicked(() => WriteLine("Button2 clicked"))))
                                        .SideEffect(b => b.GetObject<ButtonHandle>("quit", b => b
                                            .OnClicked(() => w.CloseWindow()))))
                                .Show()))))
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

// Custom GObject ========================================================================================================================
class TDoubleClass(GTypeEnum parent, string name, Func<nint, TDouble> constructor)
    : SubClass<GObjectHandle>(parent, name, constructor) { }

class TDouble(nint obj) : SubClassInst<GObjectHandle>(obj)
{
    protected override void OnCreate() => WriteLine("TDouble created");
    protected override void OnFinalize() => WriteLine("TDouble finalized");

    protected override GObjectHandle CreateHandle(nint obj) => new(obj);
}

// Custom Button ========================================================================================================================

class CustomButtonClass(GTypeEnum parent, string name, Func<nint, CustomButton> constructor)
    : SubClass<ButtonHandle>(parent, name, constructor)
{
    const int PROP_TITLE = 1;

    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        RegisterProperty(cls, 1, "testtitle");
    }
}

class CustomButton(nint obj) : SubClassInst<ButtonHandle>(obj)
{
    protected override void OnCreate() =>
        Handle.OnClicked(() =>
        {
            WriteLine($"testtitle: {testTitle}");
            Handle.Label($"{++count} times clicked");
            testTitle = Handle.GetLabel();
            GObject.Notify(Handle, "testtitle");
        });

    protected override void OnFinalize() => WriteLine("Button finalized");
    protected override ButtonHandle CreateHandle(nint obj) => new(obj);

    protected override void OnSetProperty(uint propId, nint value)
    {
        if (propId == 1)
            testTitle = GValue.GetString(value);
    }

    protected override void OnGetProperty(uint propId, nint value)
    {
        if (propId == 1)
            GValue.SetString(value, testTitle);
    }

    string? testTitle;

    int count;
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

