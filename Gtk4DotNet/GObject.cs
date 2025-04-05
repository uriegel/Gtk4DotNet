using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class GObject
{
    public static void SetString(this ObjectHandle obj, string name, string? value)
        => SetString(obj, name, value ?? "", IntPtr.Zero);
    public static string? GetString(this ObjectHandle obj, string name)
    {
        GetString(obj, name, out var value, IntPtr.Zero);
        var result = Marshal.PtrToStringUTF8(value);
        value.Free();
        return result;
    }

    /// <summary>
    /// Adds a weak reference callback to an object. Weak references are used for notification when an object is disposed. They are called “weak references” 
    /// because they allow you to safely hold a pointer to an object without calling g_object_ref() (g_object_ref() adds a strong reference, that is, 
    /// forces the object to stay alive).
    /// Note that the weak references created by this method are not thread-safe: they cannot safely be used in one thread if the object’s last g_object_unref() might happen in another thread. Use GWeakRef if thread-safety is required.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="onDisposing">Is called, when the obeject is disposed</param>
    public static THandle AddWeakRef<THandle>(this THandle obj, Action onDisposing)
        where THandle : ObjectHandle, new()
        => obj.SideEffect(o => o.AddWeakRefRaw(onDisposing));

    /// <summary>
    /// Increase the reference count of object, and possibly remove the [floating][floating-ref] reference, 
    /// if object has a floating reference. 
    /// In other words, if the object is floating, then this call “assumes ownership” of the floating reference, 
    /// converting it to a normal reference by clearing the floating flag while leaving the reference count unchanged. If the object is not floating, then this call adds a new normal reference increasing the reference count by one.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static void RefSink(this ObjectFloatingHandle obj)
    {
        obj.RefSink();
        _RefSink(obj);
    }

    /// <summary>
    /// Increase the reference count of object, and possibly remove the [floating][floating-ref] reference, 
    /// if object has a floating reference. 
    /// In other words, if the object is floating, then this call “assumes ownership” of the floating reference, 
    /// converting it to a normal reference by clearing the floating flag while leaving the reference count unchanged. If the object is not floating, then this call adds a new normal reference increasing the reference count by one.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static THandle RefSink<THandle>(this THandle obj)
        where THandle : ObjectHandle
        => obj.SideEffect(o => _RefSink(o));

    public static void SetBool(this ObjectHandle obj, string name, bool value)
        => obj.SetBool(name, value, IntPtr.Zero);
    public static bool GetBool(this ObjectHandle obj, string name)
    {
        GetBool(obj, name, out var value, IntPtr.Zero);
        return value;
    }

    public static THandle OnNotify<THandle>(this THandle obj, string property, Action<THandle> onNotify)
        where THandle : ObjectHandle
        => obj.SideEffect(o => Gtk.SignalConnect<ThreePointerDelegate>(o, $"notify::{property}", (IntPtr _, IntPtr __, IntPtr ___) => onNotify(obj)));

    public static void SetProperty(this ObjectHandle obj, string propertyName, object? value)
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
        obj.SetProperty(propertyName, gv);
        GValue.Free(gv);
    }

    public static object? GetProperty(this ObjectHandle obj, string propertyName, Type type)
    {
        var gv = GValue.Allocate();
        object? result = null;
        if (type.Name == "String")
        {
            GValue.Init(gv, GTypes.String);
            obj.GetProperty(propertyName, gv);
            result = GValue.GetString(gv);
        }
        else if (type.Name == "Boolean")
        {
            GValue.Init(gv, GTypes.Boolean);
            obj.GetProperty(propertyName, gv);
            result = GValue.GetBool(gv);
        }
        else if (type.Name == "UInt32") 
        {
            GValue.Init(gv, GTypes.UInt);
            obj.GetProperty(propertyName, gv);
            result = GValue.GetUInt(gv);
        }
        else if (type.Name == "Int32") 
        {
            GValue.Init(gv, GTypes.Int);
            obj.GetProperty(propertyName, gv);
            result = GValue.GetInt(gv);
        }
        else if (type.Name == "Double") 
        {
            GValue.Init(gv, GTypes.Double);
            obj.GetProperty(propertyName, gv);
            result = GValue.GetDouble(gv);
        }
        else if (type.Name == "Float") 
        {
            GValue.Init(gv, GTypes.Float);
            obj.GetProperty(propertyName, gv);
            result = GValue.GetFloat(gv);
        }
        GValue.Free(gv);
        return result;
    }

    public static THandle BindProperty<THandle, TTargetHandle>(this THandle source, string sourceProperty, ObjectRef<TTargetHandle> target, string targetProperty, BindingFlags bindingFlags)
        where THandle : ObjectHandle, new()
        where TTargetHandle : ObjectHandle, new()
        => source.SideEffect(s => target.SetHandle<TTargetHandle>(t => s._BindProperty(sourceProperty, t, targetProperty, bindingFlags)));

    public static THandle BindProperty<THandle>(this THandle source, string sourceProperty, ObjectHandle target, string targetProperty, BindingFlags bindingFlags)
        where THandle : ObjectHandle, new()
        => source.SideEffect(s => s._BindProperty(sourceProperty, target, targetProperty, bindingFlags));

    public static THandle Binding<THandle, TSourceHandle>(this THandle target, string targetProperty, ObjectRef<TSourceHandle> source, string sourceProperty, BindingFlags bindingFlags)
        where THandle : ObjectHandle, new()
        where TSourceHandle : ObjectHandle, new()
        => target.SideEffect(t => source.SetHandle<TSourceHandle>(s => s._BindProperty(sourceProperty, t, targetProperty, bindingFlags)));

    public static THandle Binding<THandle>(this ObjectHandle target, string targetProperty, THandle source, string sourceProperty, BindingFlags bindingFlags)
        where THandle : ObjectHandle, new()
        => source.SideEffect(s => s._BindProperty(sourceProperty, target, targetProperty, bindingFlags));

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static THandle New<THandle>(GTypeHandle type)
        where THandle : ObjectHandle, new()
    {
        var obj = New(type, 0);
        var res = new THandle();
        res.SetInternalHandle(obj);
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_new", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint New(GTypeHandle type, nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "g_free", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Free(this IntPtr obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_type_from_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static GTypeHandle TypeFromName(this string objectName);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_notify", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Notify(this ObjectHandle obj, string propertyName);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_emit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EmitSignal(this ObjectHandle obj, int signalId, int detail);
    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_emit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EmitSignal(this ObjectHandle obj, int signalId, int detail, nint param1);
    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_emit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EmitSignal(this ObjectHandle obj, int signalId, int detail, nint param1, nint param2);
    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_emit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EmitSignal(this ObjectHandle obj, int signalId, int detail, nint param1, nint param2, nint param3);
    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_emit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EmitSignal(ObjectHandle obj, int signalId, int detail, nint param1, nint param2, nint param3, nint param4);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetData(this ObjectHandle obj, string key, nint data);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint GetData(this ObjectHandle obj, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint GetData(this nint obj, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_ref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Ref(this ObjectHandle obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_data", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SetData(this nint obj, string key, nint data);

    internal static void AddWeakRefRaw(this ObjectHandle obj, Action dispose)
    {
        var key = GtkDelegates.GetKey();
        TwoPointerDelegate callback = (_, ___) =>
        {
            GtkDelegates.Remove(key);
            dispose();
        };
        GtkDelegates.Add(key, callback);
        obj.AddWeakRef(Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(IntPtr obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_class_install_property", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void ClassInstallProperty(nint cls, int propertyId, IntPtr pspec);

    [DllImport(Libs.LibGtk, EntryPoint = "g_param_spec_string", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint ParamSpecString(string name, string? nick, string? blurb, string? defaultValue, ParamFlags flags);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetString(this ObjectHandle obj, string name, string value, IntPtr end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetString(this ObjectHandle obj, string name, out IntPtr value, IntPtr end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBool(this ObjectHandle GtkHandle, string name, bool value, IntPtr end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetBool(this ObjectHandle GtkHandle, string name, out bool value, IntPtr end);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_bind_property", CallingConvention = CallingConvention.Cdecl)]
    extern static void _BindProperty(this ObjectHandle source, string sourceProperty, ObjectHandle target, string targetProperty, BindingFlags bindingFlags);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_bind_property_full", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr BindPropertyFull(this ObjectHandle source, string sourceProperty, ObjectHandle target, string targetProperty, BindingFlags bindingFlags);


    // [DllImport(Libs.LibGtk, EntryPoint="g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Unref(this IntPtr obj);

    // [DllImport(Libs.LibGtk, EntryPoint="g_clear_object", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Clear(this IntPtr obj);

    // public static void AddWeakRef(this IntPtr obj, FinalizerDelegate finalizer) => AddWeakRef(obj, finalizer, IntPtr.Zero);

    // public static void SetInt(this IntPtr obj, string name, int value)
    //     => SetInt(obj, name, value, IntPtr.Zero);
    // public static int GetInt(this IntPtr obj, string name)
    // {
    //     GetInt(obj, name, out var value, IntPtr.Zero);
    //     return value;
    // }

    // [DllImport(Libs.LibGtk, EntryPoint="g_type_name", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr TypeName(this GType type);

    // [DllImport(Libs.LibGtk, EntryPoint="g_object_set", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetInt(IntPtr obj, string name, int value, IntPtr end);

    // [DllImport(Libs.LibGtk, EntryPoint="g_object_get", CallingConvention = CallingConvention.Cdecl)]
    // extern static bool GetInt(IntPtr obj, string name, out int value, IntPtr end);
    /// <summary>
    /// Increase the reference count of object, and possibly remove the [floating][floating-ref] reference, 
    /// if object has a floating reference. 
    /// In other words, if the object is floating, then this call “assumes ownership” of the floating reference, 
    /// converting it to a normal reference by clearing the floating flag while leaving the reference count unchanged. If the object is not floating, then this call adds a new normal reference increasing the reference count by one.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [DllImport(Libs.LibGtk, EntryPoint = "g_object_ref_sink", CallingConvention = CallingConvention.Cdecl)]
    extern static void _RefSink(this ObjectHandle obj);

    /// <summary>
    /// Adds a weak reference callback to an object. Weak references are used for notification when an object is disposed. They are called “weak references” 
    /// because they allow you to safely hold a pointer to an object without calling g_object_ref() (g_object_ref() adds a strong reference, that is, 
    /// forces the object to stay alive).
    /// Note that the weak references created by this method are not thread-safe: they cannot safely be used in one thread if the object’s last g_object_unref() might happen in another thread. Use GWeakRef if thread-safety is required.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="finalizer"></param>
    /// <param name="zero"></param>
    [DllImport(Libs.LibGtk, EntryPoint = "g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern internal static void AddWeakRef(this nint obj, IntPtr finalizer, IntPtr zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern internal static void AddWeakRef(this ObjectHandle obj, IntPtr finalizer, IntPtr zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_set_property", CallingConvention = CallingConvention.Cdecl)]
    static extern void SetProperty(this ObjectHandle obj, string name, nint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_property", CallingConvention = CallingConvention.Cdecl)]
    static extern void GetProperty(this ObjectHandle obj, string name, nint value);
}


