using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using LinqTools;

namespace GtkDotNet;

public static class Application
{
    public static IntPtr New(string id) 
        => _New(id, 0)
                .SideEffect(_ => mainThreadId = Thread.CurrentThread.ManagedThreadId);

    public static int Run(string id, Action<IntPtr> onActivate) 
        => GObjectRef
                .WithRef(New(id))
                .SideEffect(_ => mainThreadId = Thread.CurrentThread.ManagedThreadId)
                .Use(gref => gref
                                .Value
                                .Run(onActivate));

    public static int Run(this IntPtr app, Action onActivate) 
    {
        mainThreadId = Thread.CurrentThread.ManagedThreadId;
        Gtk.SignalConnect(app, "activate", onActivate);
        return _Run(app, 0, IntPtr.Zero);
    }

    public static int Run(this IntPtr app, Action<IntPtr> onActivate) 
    {
        mainThreadId = Thread.CurrentThread.ManagedThreadId;
        Gtk.SignalConnect(app, "activate", () => onActivate(app));
        return _Run(app, 0, IntPtr.Zero);
    }

    /// <summary>
    /// Starts a new GTK application for use in a non GTK (console) app, see example '7-Non Gtk app'
    /// </summary>
    public static void Start()
        => new Thread(() =>
        {
            application = Application.New("de.uriegel.gtk4dotnet");
            Application.Run(application, (() =>  window = Application.NewWindow(application)));
            GObject.Unref(application); 
        }).Start();

    /// <summary>
    /// Stops the GTK application started with 'Start'
    /// </summary>
    public static void Stop()
        => Application.Dispatch(() => {
            Window.Close(window);
            Application.Quit(application);
        });

    public static bool RegisterResources()
    {
        var assembly = Assembly.GetEntryAssembly();
        var resources = assembly.GetManifestResourceNames();
        var legacyName = $"{assembly.GetName().Name}.resources.gresource";
        var actualName = "app.gresource";
        var resourceName = resources.Contains(legacyName)
            ? legacyName
            : resources.Contains(actualName)
            ? actualName
            : null;
        if (resourceName == null)
            return false;
        var stream = assembly.GetManifestResourceStream(resourceName);
        var memIntPtr = Marshal.AllocHGlobal((int)stream.Length);
        unsafe 
        {
            var memBytePtr = (byte*)memIntPtr.ToPointer();
            var writeStream = new UnmanagedMemoryStream(memBytePtr, stream.Length, stream.Length, FileAccess.Write);
            stream.CopyTo(writeStream);
        }
        var gbytes = GBytes.New(memIntPtr, stream.Length);
        Marshal.FreeHGlobal(memIntPtr);
        var res = Resource.NewFromData(gbytes, IntPtr.Zero);
        GBytes.Unref(gbytes);
        Resource.Register(res); 
        Resource.Unref(res); 
        return true;
    }

    [DllImport(Globals.LibGtk, EntryPoint="gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewWindow(IntPtr app);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_application_add_window", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddWindow(IntPtr app, IntPtr window);

    [DllImport(Globals.LibGtk, EntryPoint="g_application_quit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Quit(IntPtr app);

    public static void AddActions(IntPtr app, IEnumerable<GtkDotNet.GtkAction> actions)
    {
        var gtkActions = actions.OfType<GtkDotNet.GtkAction>();
        foreach (var action in gtkActions)
        {
            if (action.Action != null)
            {
                Delegates.Add(action.Action);
                var simpleAction = NewAction(action.Name, null);
                action.action = simpleAction;
                Gtk.SignalConnect<Action>(simpleAction, "activate", action.Action);
                AddAction(app, simpleAction);                    
            }
            else 
            {
                // Delegates.Add(action.StateChanged);
                // var state = action.StateParameterType == "s" 
                //     ? NewString(action.State as string)
                //     : NewBool((bool)action.State == true ? -1 : 0);
                // var simpleAction = NewStatefulAction(action.Name, action.StateParameterType, state);
                // action.action = simpleAction;
                // Gtk.SignalConnect<GtkAction.StateChangedDelegate>(simpleAction, "change-state", action.StateChanged);
                // AddAction(app, simpleAction);
            }
        }

        var accelEntries = 
            actions
            .Where(n => n.Accelerator != null)
            .Select(n => new { Name = "app." + n.Name, n.Accelerator});  
        foreach (var accelEntry in accelEntries)
            Application.SetAccelsForAction(app, accelEntry.Name, new [] { accelEntry.Accelerator, null});
    }

    public static void EnableSynchronizationContext()
        => SynchronizationContext.SetSynchronizationContext(new GtkSynchronizationContext());

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
    public static void BeginInvoke(int priority, Action action)
    {
        if (mainThreadId == Thread.CurrentThread.ManagedThreadId)
            action();
        else
        {
            var key = Delegates.GetKey();
            IdleFunctionDelegate mainFunction = _ =>
            {
                action();
                mainFunction = null;
                action = null;
                Delegates.Remove(key);
                return false;
            };
            Delegates.Add(key, mainFunction);
            var delegat = mainFunction as Delegate;
            var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
            Gtk.IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
        }
    }

    delegate bool IdleFunctionDelegate(IntPtr zero);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _New(string id, int flags);

    [DllImport(Globals.LibGtk, EntryPoint="g_application_run", CallingConvention = CallingConvention.Cdecl)]
    extern static int _Run(IntPtr app, int c, IntPtr a);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_application_set_accels_for_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetAccelsForAction(IntPtr app, string action, [In] string[] accels);

    [DllImport(Globals.LibGtk, EntryPoint="g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr NewAction(string action, string p);

    [DllImport(Globals.LibGtk, EntryPoint="g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr NewStatefulAction(string action, string p, IntPtr state);

    [DllImport(Globals.LibGtk, EntryPoint="g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr NewBool(int value);

    [DllImport(Globals.LibGtk, EntryPoint="g_variant_new_int32", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr NewInt(int value);

    [DllImport(Globals.LibGtk, EntryPoint="g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr NewString(string value);

    [DllImport(Globals.LibGtk, EntryPoint="g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(IntPtr app, IntPtr action);
    
    [DllImport(Globals.LibGtk, EntryPoint="g_simple_action_set_enabled", CallingConvention = CallingConvention.Cdecl)]
    extern static void EnableAction(IntPtr action, int enabled);

    /// <summary>
    /// For usage in a non GTK app
    /// </summary> 
    static IntPtr window;
    /// <summary>
    /// For usage in a non GTK app
    /// </summary> 
    static IntPtr application;

    static int mainThreadId;
}




