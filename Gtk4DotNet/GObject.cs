using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class GObject : BaseHandle
{
    public GObject() : base() { }

    public bool IsFloating { get; set; }

    public void AddWeakRefRaw(Action dispose)
    {
        var key = GtkDelegates.GetKey();
        TwoPointerDelegate callback = (_, ___) =>
        {
            GtkDelegates.Remove(key);
            dispose();
        };
        GtkDelegates.Add(key, callback);
        AddWeakRef(this, Marshal.GetFunctionPointerForDelegate(callback as Delegate), 0);
    }

    /// <summary>
    /// Adds a weak reference callback to an object. Weak references are used for notification when an object is disposed. They are called “weak references” 
    /// because they allow you to safely hold a pointer to an object without calling g_object_ref() (g_object_ref() adds a strong reference, that is, 
    /// forces the object to stay alive).
    /// Note that the weak references created by this method are not thread-safe: they cannot safely be used in one thread if the object’s last g_object_unref() might happen in another thread. Use GWeakRef if thread-safety is required.
    /// </summary>
    /// <param name="onDisposing">Is called, when the obeject is disposed</param>
    public THandle AddWeakRef<THandle>(Action onDisposing)
        where THandle : GObject, new()
        => (THandle)this.SideEffect(o => o.AddWeakRefRaw(onDisposing));

    protected void SignalConnect<TDelegate>(string name, TDelegate callback)
        where TDelegate : Delegate
    {
        var key = GtkDelegates.GetKey();
        GtkDelegates.Add(key, callback);
        AddWeakRefRaw(() => GtkDelegates.Remove(key));
        SignalConnect(this, name, Marshal.GetFunctionPointerForDelegate((Delegate)callback), IntPtr.Zero, 0);
    }

    protected override bool ReleaseHandle()
         => IsFloating
             || true.SideEffectIf(!IsFloating, _ => Unref(handle));

    [DllImport(Libs.LibGtk, EntryPoint = "g_free", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Free(nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern internal static void AddWeakRef(GObject obj, nint finalizer, nint zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    protected extern static long SignalConnect(GObject widget, string name, IntPtr callback, IntPtr obj, int n3);
}