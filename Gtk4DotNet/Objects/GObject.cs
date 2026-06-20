using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// Release ready

/// <summary>
/// The base type system and object class for Gtk4
/// </summary>
public class GObject : BaseHandle
{
    /// <summary>
    /// The Object is owned by a parent or Gtk and is not being unreffed by this instance
    /// </summary>
    public bool AutoDestroyed { get; internal set; }

    /// <summary>
    /// Do this object has a fGtk floating ref
    /// </summary>
    public bool HasFloatingRef { get => _HasFloatingRef(this); }

    /// <summary>
    /// Add a notification action to notify when this instance is destroyed
    /// </summary>
    /// <param name="onFinalize"></param>
    public void OnFinalize(Action onFinalize) => AddWeakRef(onFinalize);

    /// <summary>
    /// The object's RefCount
    /// </summary>
    public int RefCount { get => Marshal.PtrToStructure<GObjectStruct>(GetInternalHandle()).RefCount; }

    public void SetProperty(string propertyName, object? value)
    {
        var gval = new GValue(value);
        SetProperty(this, propertyName, ref gval);
        gval.Unset();
    }

    public object? GetProperty(string propertyName, Type type)
    {
        var gval = new GValue();
        object? result = type.Name switch
        {
            "String" => gval.Init(GTypes.String).GetProperty(this, propertyName).GetString(),
            "Boolean" => gval.Init(GTypes.Boolean).GetProperty(this, propertyName).GetBool(),
            "UInt32" => gval.Init(GTypes.Boolean).GetProperty(this, propertyName).GetUInt(),
            "Int32" => gval.Init(GTypes.Boolean).GetProperty(this, propertyName).GetInt(),
            "Double" => gval.Init(GTypes.Boolean).GetProperty(this, propertyName).GetDouble(),
            "Float" => gval.Init(GTypes.Boolean).GetProperty(this, propertyName).GetFloat(),
            _ => null
        };
        gval.Unset();
        return result;
    }

    public void OnNotify(string property, Action onNotify)
        => SignalConnect<ThreePointerDelegate>($"notify::{property}", (nint _, nint __, nint ___) => onNotify());

    public void BindProperty(string property, GObject target, string targetProperty, BindingFlags flags)
        => BindProperty(this, property, target, targetProperty, flags);

    /// <summary>
    /// Sets a managed object to this GObject instance
    /// </summary>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public void SetManagedData(string key, object? obj)
    {
        var dkey = GtkDelegates.Instance.GetKey("SetManagedData");
        OnePointerDelegate callback = data =>
        {
            GCHandle.FromIntPtr(data).Free();
            GtkDelegates.Instance.Remove(dkey.Key);
        };
        GtkDelegates.Instance.Add(dkey, callback);
        SetQDataFull(this, GetQuark(key), GCHandle.ToIntPtr(GCHandle.Alloc(obj, GCHandleType.Normal)),
            Marshal.GetFunctionPointerForDelegate(callback as Delegate));
    }

    /// <summary>
    /// Gets the previously set managed data  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T? GetManagedData<T>(string key)
    {
        var p = GetQData(this, GetQuark(key));
        return p != 0 ? (T?)GCHandle.FromIntPtr(p).Target : (T?)(object?)null;
    }

    public void SetString(string name, string? value)
        => SetString(this, name, value ?? "", 0);

    public string? GetString(string name)
    {
        GetString(this, name, out var value, 0);
        return value.PtrToString(true);
    }

    public void SetBool(string name, bool value)
        => SetBool(this, name, value, 0);

    public bool GetBool(string name)
    {
        GetBool(this, name, out var value, 0);
        return value;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GType Type();

    /// <summary>
    /// Adds a weak reference callback to an object. Weak references are used for notification when an object is disposed. They are called “weak references” 
    /// because they allow you to safely hold a pointer to an object without calling g_object_ref() (g_object_ref() adds a strong reference, that is, 
    /// forces the object to stay alive).
    /// Note that the weak references created by this method are not thread-safe: they cannot safely be used in one thread if the object’s last g_object_unref() might happen in another thread. Use GWeakRef if thread-safety is required.
    /// </summary>
    /// <param name="onDisposing">Is called, when the obeject is disposed</param>
    internal void AddWeakRef(Action onDisposing)
    {
        var key = GtkDelegates.Instance.GetKey("WeakRef");
        TwoPointerDelegate callback = (_, ___) =>
        {
            GtkDelegates.Instance.Remove(key.Key);
            onDisposing();
        };
        GtkDelegates.Instance.Add(key, callback, GetType().FullName);
        _AddWeakRef(this, Marshal.GetFunctionPointerForDelegate(callback as Delegate), 0);
    }

    internal static GtkDelegates GObjectsDiagnostics { get; } = new();

    internal void CheckDiagnostics()
    {
        if (!IsInvalid && Gtk.Diagnostics && !diagnosticsSet)
            SetDiagnostics();
    }

    internal DelegateId SignalConnect<TDelegate>(string name, TDelegate callback, bool manualFreeing = false)
        where TDelegate : Delegate
    {
        var key = GtkDelegates.Instance.GetKey($"Signal: {name}");
        GtkDelegates.Instance.Add(key, callback, GetType().FullName);
        if (!manualFreeing)
            AddWeakRef(() => GtkDelegates.Instance.Remove(key.Key));
        key.SignalId = SignalConnect(this, name, Marshal.GetFunctionPointerForDelegate((Delegate)callback), 0, 0);
        return key;
    }

    internal void SignalDisconnect(DelegateId id)
    {
        SignalDisconnect(this, id.SignalId);
        GtkDelegates.Instance.Remove(id.Key);
    }

    protected virtual void OnDiagnostics()
        => Console.WriteLine($"{GetType().Name} finalized");

    protected override bool ReleaseHandle()
    {
        if (!AutoDestroyed)
            Unref(handle);
        return true;
    }

    void SetDiagnostics()
    {
        diagnosticsSet = true;
        var key = GObjectsDiagnostics.GetKey("SetDiagnostics");
        TwoPointerDelegate callback = (_, ___) =>
        {
            GObjectsDiagnostics.Remove(key.Key);
            if (Gtk.GObjectTracing)
                OnDiagnostics();
        };
        GObjectsDiagnostics.Add(key, callback, GetType().FullName);
        _AddWeakRef(this, Marshal.GetFunctionPointerForDelegate(callback as Delegate), 0);
    }


    [DllImport(Libs.LibGtk, EntryPoint = "g_free", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Free(nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern internal static void _AddWeakRef(GObject obj, nint finalizer, nint zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_add_toggle_ref", CallingConvention = CallingConvention.Cdecl)]
    extern internal static void _AddToggleRef(GObject obj, nint finalizer, nint zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    protected extern static long SignalConnect(GObject obj, string name, nint callback, nint o, int n3);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_handler_disconnect", CallingConvention = CallingConvention.Cdecl)]
    protected extern static void SignalDisconnect(GObject obj, long signalId);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_data", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetData(GObject obj, string key, nint data);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_data", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetData(GObject obj, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_property", CallingConvention = CallingConvention.Cdecl)]
    static extern void SetProperty(GObject obj, string name, ref GValue value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_bind_property", CallingConvention = CallingConvention.Cdecl)]
    static extern nint BindProperty(GObject source, string property, GObject target, string targetProperty, BindingFlags flags);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetString(GObject obj, string name, string value, nint end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetString(GObject obj, string name, out nint value, nint end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBool(GObject obj, string name, bool value, nint end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetBool(GObject obj, string name, out bool value, nint end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_qdata_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetQDataFull(GObject obj, int quark, nint p, nint destroyNotify);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_qdata", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetQData(GObject obj, int quark);

    [DllImport(Libs.LibGtk, EntryPoint = "g_quark_from_string", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetQuark(string quark);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_is_floating", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _HasFloatingRef(GObject obj);

    bool diagnosticsSet;
}

public static class GObjectExtensions
{
    /// <summary>
    /// Adds a weak reference callback to an object. Weak references are used for notification when an object is disposed. They are called “weak references” 
    /// because they allow you to safely hold a pointer to an object without calling g_object_ref() (g_object_ref() adds a strong reference, that is, 
    /// forces the object to stay alive).
    /// Note that the weak references created by this method are not thread-safe: they cannot safely be used in one thread if the object’s last g_object_unref() might happen in another thread. Use GWeakRef if thread-safety is required.
    /// </summary>
    /// <param name="obj">The GObject instance</param>
    /// <param name="onDisposing">Is called, when the obeject is disposed</param>
    public static THandle AddWeakRef<THandle>(this GObject obj, Action onDisposing)
        where THandle : GObject, new()
        => (THandle)obj.SideEffect(o => o.AddWeakRef(onDisposing));

    public static THandle Notify<THandle>(this THandle obj, string property, Action onNotify)
        where THandle : GObject
        => obj.SideEffect(o => o.OnNotify(property, onNotify));

    public static THandle Finalize<THandle>(this THandle obj, Action onFinalize)
        where THandle : GObject
        => obj.SideEffect(o => o.OnFinalize(onFinalize));
}