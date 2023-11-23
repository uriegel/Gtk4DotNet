using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Gtk
{
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
            var key = Delegates.GetKey();
            OnePointerBoolRetDelegate? mainFunction = _ =>
            {
                action?.Invoke();
                mainFunction = null;
                action = null;
                Delegates.Remove(key);
                return false;
            };
            Delegates.Add(key, mainFunction);
            var delegat = mainFunction as Delegate;
            var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
            IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
        }
    }

    internal static void Init() => 
        SynchronizationContext.SetSynchronizationContext(
            new GtkSynchronizationContext()
                .SideEffect(_ => mainThreadId = Environment.CurrentManagedThreadId));
     
    internal static void SignalConnect<TDelegate>(IntPtr action, string name, TDelegate callback) where TDelegate : Delegate
    {
        var delegat = callback as Delegate;
        var id = SignalConnectAction(action, name, Marshal.GetFunctionPointerForDelegate(callback), IntPtr.Zero, IntPtr.Zero, 0);
        Delegates.Add(id, delegat);
    }

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnect(this GtkHandle widget, string name, IntPtr callback, IntPtr obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_data", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnect(this GtkHandle widget, string name, IntPtr callback, IntPtr n, IntPtr n2, int n3);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Main();

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main_quit", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void MainQuit();

    // [DllImport(Libs.LibGtk, EntryPoint="g_content_type_guess", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr GuessContentType(string filename, IntPtr nil1,  IntPtr nil2, IntPtr nil3);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_init", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Init (ref int argc, ref IntPtr argv);
   
    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_data", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnectAfter(this GtkHandle widget, string name, IntPtr callback, IntPtr n, IntPtr n2, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_handler_disconnect", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SignalDisconnect(this GtkHandle widget, long id);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_data", CallingConvention = CallingConvention.Cdecl)]
    extern static long SignalConnectAction(IntPtr action, string name, IntPtr callback, IntPtr n, IntPtr n2, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_idle_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void IdleAddFull(int priority, IntPtr func, IntPtr nil, IntPtr nil2);

    static int mainThreadId;
}

