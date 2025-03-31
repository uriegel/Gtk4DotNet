using System.Reflection;
using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;
using GtkDotNet.SubClassing;

namespace GtkDotNet;

public static class Application
{
    public static ApplicationHandle New(string id, int flags = 0)
        => _New(id, 0)
                .SideEffect(_ => Gtk.Init());

    public static ApplicationHandle NewAdwaita(string id, int flags = 0)
        => _NewAdw(id, 0)
                .SideEffect(_ => Gtk.Init());

    public static ApplicationHandle SubClass<THandle>(this ApplicationHandle app, SubClass<THandle> subClass)
            where THandle : ObjectHandle, new()
        => app.SideEffect(_ => subClasses.Add(subClass));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ApplicationWindowHandle NewWindow(this ApplicationHandle app);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ApplicationWindowHandle NewAdwaitaWindow(this ApplicationHandle app);

    public static ApplicationWindowHandle CustomWindow(this ApplicationHandle app, string customWindow)
    {
        var window = GObject.New<ApplicationWindowHandle>(customWindow.TypeFromName());
        window.SetApplication(app);
        var mw = Controls.ManagedApplicationWindow.GetInstance(window.GetInternalHandle()) as Controls.ManagedApplicationWindow;
        mw?.Initialize();
        return window;
    }

    public static AdwApplicationWindowHandle CustomAdwWindow(this ApplicationHandle app, string customWindow)
    {
        var window = GObject.New<AdwApplicationWindowHandle>(customWindow.TypeFromName());
        window.SetApplication(app);
        var mw = Controls.ManagedAdwApplicationWindow.GetInstance(window.GetInternalHandle()) as Controls.ManagedAdwApplicationWindow;
        mw?.Initialize();
        return window;
    }

    public static ApplicationWindowHandle ManagedApplicationWindow(this ApplicationHandle app)
        => CustomWindow(app, MANAGED_APPLICATION_WINDOW);

    public static AdwApplicationWindowHandle ManagedAdwApplicationWindow(this ApplicationHandle app)
        => CustomAdwWindow(app, MANAGED_ADW_APPLICATION_WINDOW);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_add_window", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddWindow(this ApplicationHandle app, WindowHandle window);

    public static int Run(this ApplicationHandle app, int c, IntPtr a)
    {
        var result = app._Run(c, a);
        app.Dispose();
        return result;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_application_quit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Quit(this ApplicationHandle app);

    [DllImport(Libs.LibGtk, EntryPoint = "g_application_run", CallingConvention = CallingConvention.Cdecl)]
    extern static int _Run(this ApplicationHandle app, int c, IntPtr a);

    public static ApplicationHandle OnActivate(this ApplicationHandle app, Action<ApplicationHandle> activate)
        => app.SideEffect(a => Gtk.SignalConnect<OnePointerDelegate>(a, "activate", (IntPtr _) => activate(app)));

    public static bool RegisterResources()
    {
        var assembly = Assembly.GetEntryAssembly();
        var resources = assembly?.GetManifestResourceNames();
        var legacyName = $"{assembly?.GetName().Name}.resources.gresource";
        var actualName = "app.gresource";
        var resourceName = resources?.Contains(legacyName) == true
            ? legacyName
            : resources?.Contains(actualName) == true
            ? actualName
            : null;
        if (resourceName == null)
            return false;
        var stream = assembly?.GetManifestResourceStream(resourceName);
        var memIntPtr = Marshal.AllocHGlobal((int)(stream?.Length ?? 0));
        unsafe
        {
            var memBytePtr = (byte*)memIntPtr.ToPointer();
            var writeStream = new UnmanagedMemoryStream(memBytePtr, stream?.Length ?? 0, stream?.Length ?? 0, FileAccess.Write);
            stream?.CopyTo(writeStream);
        }
        using var gbytes = GBytes.New(memIntPtr, stream?.Length ?? 0);
        Marshal.FreeHGlobal(memIntPtr);
        using var res = Resource.NewFromData(gbytes);
        Resource.Register(res);
        return true;
    }

    internal const string MANAGED_APPLICATION_WINDOW = "ManagedApplicationWindow";
    internal const string MANAGED_ADW_APPLICATION_WINDOW = "ManagedAdwApplicationWindow";

    [DllImport(Libs.LibAdw, EntryPoint = "adw_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationHandle _NewAdw(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationHandle _New(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_set_accels_for_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetAccelsForAction(ApplicationHandle app, string action, [In] string?[] accels);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr NewBool(int value);

    // [DllImport(Libs.LibGtk, EntryPoint="g_variant_new_int32", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr NewInt(int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr NewString(string value);

    static readonly List<object> subClasses = [];
}
