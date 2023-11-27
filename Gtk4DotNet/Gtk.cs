using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

// TODO Extended Samples in Branch "OldVersion"

public static class Gtk
{
    /// <summary>
    /// Starts a new GTK application for use in a non GTK (console) app, see example '7-Non Gtk app'
    /// </summary>
    public static void Start()
        => new Thread(() =>
        {
            nonGtkApp = Application.New("de.uriegel.gtk4dotnet");
            nonGtkApp.OnActivate(a => nonGtkWindow = a.NewWindow());
            nonGtkApp.Run(0, IntPtr.Zero);
            nonGtkApp.Dispose();
        }).Start();

    /// <summary>
    /// Stops the GTK application started with 'Start'
    /// </summary>
    public static void Stop()
        => Dispatch(() => {
            nonGtkWindow?.Close();
            nonGtkApp?.Quit();
        });

    public static Task Dispatch(Action action, bool highPriority = false)
        => Dispatch(action, highPriority ? 100 : 200);

    public static Task Dispatch(Action action, int priority)
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        BeginInvoke(priority, () =>
        {
            try
            {
                action();
                tcs.TrySetResult();
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        });
        return tcs.Task;
    }

    public static Task<T> Dispatch<T>(Func<T> action, bool highPriority = false)
        => Dispatch(action, highPriority ? 100 : 200);

    public static Task<T> Dispatch<T>(Func<T> action, int priority)
    {
        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        BeginInvoke(priority, () => 
        {
            try
            {
                tcs.TrySetResult(action());
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        });
        return tcs.Task;
    }

    /// <summary>
    /// Run the specified Action in the main GTK thread
    /// </summary>
    /// <param name="priority">Between 100 (high), 200 (idle) and 300 (low)</param>
    /// <param name="action">Action which runs in main thread</param>
    public static void BeginInvoke(int priority, Action? action)
    {
        if (mainThreadId == Environment.CurrentManagedThreadId)
            action?.Invoke();
        else
        {
            var key = GtkDelegates.GetKey();
            OnePointerBoolRetDelegate? mainFunction = _ =>
            {
                action?.Invoke();
                mainFunction = null;
                action = null;
                GtkDelegates.Remove(key);
                return false;
            };
            GtkDelegates.Add(key, mainFunction);
            var delegat = mainFunction as Delegate;
            var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
            IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
        }
    }

    public static string? GuessContentType(string filename)
        => GuessContentType(filename, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero).PtrToString(true);


    internal static void Init() => 
        SynchronizationContext.SetSynchronizationContext(
            new GtkSynchronizationContext()
                .SideEffect(_ => mainThreadId = Environment.CurrentManagedThreadId));

    internal static long SignalConnect<TDelegate>(ObjectHandle obj, string name, TDelegate callback)
        where TDelegate : Delegate
    {
        // TODO Signal disconnect
        var key = GtkDelegates.GetKey();
        GtkDelegates.Add(key, callback);
        obj.AddWeakRefRaw(() => GtkDelegates.Remove(key));
        return SignalConnect(obj, name, Marshal.GetFunctionPointerForDelegate((Delegate)callback), IntPtr.Zero, 0);
    }

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    extern static long SignalConnect(this ObjectHandle widget, string name, IntPtr callback, IntPtr obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnectAction(IntPtr action, string name, IntPtr callback, IntPtr obj, int n3);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Main();

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main_quit", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void MainQuit();

    [DllImport(Libs.LibGtk, EntryPoint="g_content_type_guess", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GuessContentType(string filename, IntPtr nil1,  IntPtr nil2, IntPtr nil3);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_init", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Init (ref int argc, ref IntPtr argv);
   
    [DllImport(Libs.LibGtk, EntryPoint="g_signal_handler_disconnect", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SignalDisconnect(this ObjectHandle widget, long id);

    [DllImport(Libs.LibGtk, EntryPoint="g_idle_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void IdleAddFull(int priority, IntPtr func, IntPtr nil, IntPtr nil2);

    /// <summary>
    /// For usage in a non GTK app
    /// </summary> 
    static WindowHandle? nonGtkWindow;
    /// <summary>
    /// For usage in a non GTK app
    /// </summary> 
    static ApplicationHandle? nonGtkApp;

    static int mainThreadId;
}

