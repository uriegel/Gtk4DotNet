using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class GObject : BaseHandle
{
    public bool IsFloating { get; set; }

    public void OnFinalize(Action onFinalize) => AddWeakRef(onFinalize);

    /// <summary>
    /// Adds a weak reference callback to an object. Weak references are used for notification when an object is disposed. They are called “weak references” 
    /// because they allow you to safely hold a pointer to an object without calling g_object_ref() (g_object_ref() adds a strong reference, that is, 
    /// forces the object to stay alive).
    /// Note that the weak references created by this method are not thread-safe: they cannot safely be used in one thread if the object’s last g_object_unref() might happen in another thread. Use GWeakRef if thread-safety is required.
    /// </summary>
    /// <param name="onDisposing">Is called, when the obeject is disposed</param>
    public void AddWeakRef(Action onDisposing)
    {
        var key = GtkDelegates.GetKey();
        TwoPointerDelegate callback = (_, ___) =>
        {
            GtkDelegates.Remove(key);
            onDisposing();
        };
        GtkDelegates.Add(key, callback);
        _AddWeakRef(this, Marshal.GetFunctionPointerForDelegate(callback as Delegate), 0);
    }

    public void SetData(string key, nint data) => SetData(this, key, data);

    public nint GetData(string key) => GetData(this, key);

    public void SetProperty(string propertyName, object? value)
    {
        var gv = GValue.Allocate();
        if (value is string s)
        {
            GValue.Init(gv, GTypes.String);
            GValue.SetString(gv, s);
        }
        else if (value is bool b)
        {
            GValue.Init(gv, GTypes.Boolean);
            GValue.SetBool(gv, b);
        }
        else if (value is int n)
        {
            GValue.Init(gv, GTypes.Int);
            GValue.SetInt(gv, n);
        }
        else if (value is uint u)
        {
            GValue.Init(gv, GTypes.UInt);
            GValue.SetUInt(gv, u);
        }
        else if (value is double d)
        {
            GValue.Init(gv, GTypes.Double);
            GValue.SetDouble(gv, d);
        }
        else if (value is float f)
        {
            GValue.Init(gv, GTypes.Float);
            GValue.SetFloat(gv, f);
        }
        else
            GValue.Init(gv, GTypes.String);
        SetProperty(this, propertyName, gv);
        GValue.Free(gv);
    }

    public object? GetProperty(string propertyName, Type type)
    {
        var gv = GValue.Allocate();
        object? result = null;
        if (type.Name == "String")
        {
            GValue.Init(gv, GTypes.String);
            GetProperty(this, propertyName, gv);
            result = GValue.GetString(gv);
        }
        else if (type.Name == "Boolean")
        {
            GValue.Init(gv, GTypes.Boolean);
            GetProperty(this, propertyName, gv);
            result = GValue.GetBool(gv);
        }
        else if (type.Name == "UInt32") 
        {
            GValue.Init(gv, GTypes.UInt);
            GetProperty(this, propertyName, gv);
            result = GValue.GetUInt(gv);
        }
        else if (type.Name == "Int32") 
        {
            GValue.Init(gv, GTypes.Int);
            GetProperty(this, propertyName, gv);
            result = GValue.GetInt(gv);
        }
        else if (type.Name == "Double") 
        {
            GValue.Init(gv, GTypes.Double);
            GetProperty(this, propertyName, gv);
            result = GValue.GetDouble(gv);
        }
        else if (type.Name == "Float") 
        {
            GValue.Init(gv, GTypes.Float);
            GetProperty(this, propertyName, gv);
            result = GValue.GetFloat(gv);
        }
        GValue.Free(gv);
        return result;
    }

    public void OnNotify(string property, Action onNotify)
        => SignalConnect<ThreePointerDelegate>($"notify::{property}", (nint _, nint __, nint ___) => onNotify());

    public void BindProperty(string property, GObject target, string targetProperty, BindingFlags flags)
        => BindProperty(this, property, target, targetProperty, flags);

    internal void CheckDiagnostics()
    {
        if (!IsInvalid && Gtk.Diagnostics && !diagnosticsSet)
        {
            OnFinalize(OnFinalization);
            diagnosticsSet = true;
        }
    }

    internal void SignalConnect<TDelegate>(string name, TDelegate callback)
        where TDelegate : Delegate
    {
        var key = GtkDelegates.GetKey();
        GtkDelegates.Add(key, callback);
        AddWeakRef(() => GtkDelegates.Remove(key));
        SignalConnect(this, name, Marshal.GetFunctionPointerForDelegate((Delegate)callback), 0, 0);
    }

    protected virtual void OnFinalization()
        => Console.WriteLine($"{GetType().Name} finalized");

    protected override bool ReleaseHandle()
         => IsFloating
             || true.SideEffectIf(!IsFloating, _ => Unref(handle));

    [DllImport(Libs.LibGtk, EntryPoint = "g_free", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Free(nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern internal static void _AddWeakRef(GObject obj, nint finalizer, nint zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    protected extern static long SignalConnect(GObject widget, string name, IntPtr callback, IntPtr obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_data", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetData(GObject obj, string key, nint data);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_data", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetData(GObject obj, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_property", CallingConvention = CallingConvention.Cdecl)]
    static extern void SetProperty(GObject obj, string name, nint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_property", CallingConvention = CallingConvention.Cdecl)]
    static extern void GetProperty(GObject obj, string name, nint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_bind_property", CallingConvention = CallingConvention.Cdecl)]
    static extern nint BindProperty(GObject source, string property, GObject target, string targetProperty, BindingFlags flags);

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