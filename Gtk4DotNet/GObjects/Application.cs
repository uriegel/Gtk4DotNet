using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// TODO OnCommandLine

/// <summary>
/// The GTK Application. 
/// </summary>
public class Application : GObject
{
    public static GSettings Settings
    {
        get => field ?? throw new Exception("Settings not initialized 'WithSettings'");
        private set;
    }
    
    public static string ApplicationId { get; private set; } = null!;

    /// <summary>
    /// Creates a new GtkApplication. It is not neccessary to call Gtk.Init
    /// </summary>
    /// <param name="applicationId">The Gtk Application ID this appltcation is connected to</param>
    /// <param name="flags"></param>
    /// <returns>Application for chaining calls</returns>
    public static Application New(string applicationId, ApplicationFlags flags = ApplicationFlags.None)
    {
        var app = _New(applicationId, flags);
        ApplicationId = applicationId;
        Gtk.Init();
        return app;
    }

    /// <summary>
    /// Creates a new Adwaita application
    /// </summary>
    /// <param name="applicationId">The Gtk Application ID this application is connected to</param>
    /// <param name="flags"></param>
    /// <returns>Application for chaining calls</returns>
    public static Application NewAdwaita(string applicationId, ApplicationFlags flags = ApplicationFlags.None)
    {
        var app = _NewAdw(applicationId, flags);
        ApplicationId = applicationId;
        Gtk.Init();
        return app;
    }

    /// <summary>
    /// The most neccesary callback: when the application is activated, the main window should be created.
    /// </summary>
    /// <param name="activate"></param>
    /// <returns>Application for chaining calls</returns>
    public Application OnActivate(Action<Application> activate)
        => this.SideEffect(_ => SignalConnect<OnePointerDelegate>("activate", _ => activate(this)));

    public Application OnOpen(Action<Application, GFile[]> onOpen)
    {
        SignalConnect<FivePointerDelegate>("open", (_, filesPtr, n, _, _) =>
        {
            var files = Enumerable.Range(0, (int)n).Select(n =>
            {
                var p = Marshal.ReadIntPtr(filesPtr, n * IntPtr.Size);
                var gfile = new GFile();
                gfile.SetInternalHandle(p);
                gfile.WeakCopy = true;
                gfile.CheckDiagnostics();
                return gfile;

            }).ToArray();
            onOpen(this, files);
        });
        return this;
    }

    /// <summary>
    /// When Diagnostics are switched on, a report of probably not released delegates or object is displayed in the console. 
    /// This method has to called before other GObject base types are created.
    /// </summary>
    /// <param name="gobjectTracing">Eyery time a GObject is freed, this will be logged</param>
    /// <returns>Application for chaining calls</returns>
    public Application WithDiagnostics(bool gobjectTracing = false)
    {
        Gtk.Diagnostics = true;
        Gtk.GObjectTracing = gobjectTracing;
        CheckDiagnostics();
        Console.WriteLine($"Running process: {Environment.ProcessId}");
        return this;
    }

    /// <summary>
    /// Using globally GSettings via the static <see cref="Settings"/>. There has to be a gschema.xml present an a build chain in the csproj project file,
    /// see README.md in https://github.com/uriegel/Gtk4DotNet/blob/Main/README.md
    /// </summary>
    /// <returns>Application for chaining calls</returns>
    public Application WithSettings()
    {
        Settings = GSettings.NewFromResource(ApplicationId, true);
        return this;
    }

    /// <summary>
    /// If a Webkit Webview is used and defined in a template.ui this method has to be called to register the WebKit for using with a builder
    /// </summary>
    /// <returns>Application for chaining calls</returns>
    public Application WithWebKit()
    {
        var t = WebView.Type();
        return this;
    }

    /// <summary>
    /// This runs the application. The <see cref="OnActivate"/> callback is being called, and the application runs as long as the main window is present.
    /// </summary>
    /// <returns>Exit status</returns>
    public int Run()
    {
        var args = Environment.GetCommandLineArgs();
        // Allocate argv array
        var argvPtrs = new IntPtr[args.Length];
        for (int i = 0; i < args.Length; i++)
            argvPtrs[i] = Marshal.StringToHGlobalAnsi(args[i]);
        var argv = Marshal.AllocHGlobal(IntPtr.Size * args.Length);
        for (int i = 0; i < args.Length; i++)
            Marshal.WriteIntPtr(argv, i * IntPtr.Size, argvPtrs[i]);
        var result = _Run(this, args.Length, argv);

        // Cleanup
        for (int i = 0; i < args.Length; i++)
            Marshal.FreeHGlobal(argvPtrs[i]);
        Marshal.FreeHGlobal(argv);

        Dispose();
        if (Gtk.Diagnostics)
            Gtk.ShowDiagnostics();
        return result;
    }

    /// <summary>
    /// Creates a new <see cref="ApplicationWindow"/>
    /// </summary>
    /// <returns>The newly created <see cref="ApplicationWindow"/></returns>
    public ApplicationWindow NewWindow()
    {
        var res = NewWindow(this);
        res.CheckDiagnostics();
        return res;
    }

    /// <summary>
    /// Creates a new <see cref="ApplicationWindow"/> from a template.ui. This template has to be included as a .NET resource
    /// </summary>
    /// <param name="template">The resource name of the tempplate.ui</param>
    /// <param name="window">The name of the ApplicationWindow in the template</param>
    /// <param name="creator">A function to create an <see cref="ApplicationWindow"/>, or a custom window inherited from <see cref="ApplicationWindow"/></param>
    /// <returns>The newly created <see cref="ApplicationWindow"/></returns>
    public ApplicationWindow WindowFromBuilder(string template, string window, Func<WindowBuilder, ApplicationWindow> creator)
    {
        using var builder = Builder.FromDotNetResource(template);
        var res = creator(new(window, builder, this));
        res.CheckDiagnostics();
        return res;
    }

    /// <summary>
    /// Adds an array of <see cref="GtkAction"/> to this ActionMap.
    /// </summary>
    /// <remarks>
    /// Important: when setting actions with shortcuts, add those with more specific shortcuts like <c>&lt;Ctrl&gt;F3</c>  b e f o r e  those with less specific shortcuts like <c>F3</c>. 
    /// </remarks>
    /// <param name="actions">An array of <see cref="GtkAction"/> to be added to the application</param>
    public void AddActions(params GtkAction[] actions) => this.actions.AddActions(this, this, "app", actions);

    /// <summary>
    /// Adds a <see cref="Window"/> to the application. This will keep the application alive until this (and other added windows) will be closed.
    /// </summary>
    /// <param name="window">A window to add to the application</param>
    public void AddWindow(Window window) => AddWindow(this, window);

    /// <summary>
    /// Removes a <see cref="Window" /> from the application. The application may stop running as a result of a call to this function, if the window was the last window of the application.
    /// </summary>
    /// <param name="window">The window to remove from the application</param>
    public void RemoveWindow(Window window) => RemoveWindow(this, window);

    public void SetAccelsForAction(string action, [In] string?[] accels) => SetAccelsForAction(this, action, accels);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Application _NewAdw(string id, ApplicationFlags flags);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Application _New(string id, ApplicationFlags flags);

    [DllImport(Libs.LibGtk, EntryPoint = "g_application_run", CallingConvention = CallingConvention.Cdecl)]
    extern static int _Run(Application app, int c, nint a);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationWindow NewWindow(Application app);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_set_accels_for_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetAccelsForAction(Application app, string action, [In] string?[] accels);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_add_window", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddWindow(Application app, Window window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_remove_window", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveWindow(Application app, Window window);

    readonly GtkActions actions = new(false);
}

public static class ApplicationExtensions
{

    /// <summary>
    /// Adds an array of <see cref="GtkAction"/> to this ActionMap.
    /// </summary>
    /// <remarks>
    /// Important: when setting actions with shortcuts, add those with more specific shortcuts like <c>&lt;Ctrl&gt;F3</c>  b e f o r e  those with less specific shortcuts like <c>F3</c>. 
    /// </remarks>
    /// <param name="app"></param>
    /// <param name="actions">An array of <see cref="GtkAction"/> to be added to the application</param>
    /// <returns>Application for chaining calls</returns>
    public static TApplication Actions<TApplication>(this TApplication app, params GtkAction[] actions)
        where TApplication : Application
        => app.SideEffect(app => app.AddActions(actions));

    public static TApplication AccelsForAction<TApplication>(this TApplication app, string action, string?[] accels)
        where TApplication : Application
        => app.SideEffect(app => app.SetAccelsForAction(action, accels));
}

/// <summary>
/// A combination of a window builder, the name of the window in the template.ui, and the application
/// </summary>
/// <param name="Window"></param>
/// <param name="Builder"></param>
/// <param name="Application"></param>
public record WindowBuilder(string Window, Builder Builder, Application Application);