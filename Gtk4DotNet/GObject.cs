using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class GObject
{
    [DllImport(Globals.LibGtk, EntryPoint="g_object_ref", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Ref(IntPtr obj);

    /// <summary>
    /// Increase the reference count of object, and possibly remove the [floating][floating-ref] reference, 
    /// if object has a floating reference. 
    /// In other words, if the object is floating, then this call “assumes ownership” of the floating reference, 
    /// converting it to a normal reference by clearing the floating flag while leaving the reference count unchanged. If the object is not floating, then this call adds a new normal reference increasing the reference count by one.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [DllImport(Globals.LibGtk, EntryPoint="g_object_ref_sink", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr RefSink(IntPtr obj);
    
    [DllImport(Globals.LibGtk, EntryPoint="g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Unref(IntPtr obj);

    [DllImport(Globals.LibGtk, EntryPoint="g_clear_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Clear(IntPtr obj);

    [DllImport(Globals.LibGtk, EntryPoint="g_free", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Free(IntPtr obj);

    public static void AddWeakRef(IntPtr obj, FinalizerDelegate finalizer) => AddWeakRef(obj, finalizer, IntPtr.Zero);

    public static void SetBool(IntPtr obj, string name, bool value)
        => SetBool(obj, name, value, IntPtr.Zero);
    public static bool GetBool(IntPtr obj, string name)
    {
        GetBool(obj, name, out var value, IntPtr.Zero);
        return value;
    }

    public static void SetInt(IntPtr obj, string name, int value)
        => SetInt(obj, name, value, IntPtr.Zero);
    public static int GetInt(IntPtr obj, string name)
    {
        GetInt(obj, name, out var value, IntPtr.Zero);
        return value;
    }

    [DllImport(Globals.LibGtk, EntryPoint="g_object_set", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetString(IntPtr obj, string name, string value, IntPtr end);

    public delegate void FinalizerDelegate(IntPtr zero, IntPtr obj);

    [DllImport(Globals.LibGtk, EntryPoint="g_object_bind_property", CallingConvention = CallingConvention.Cdecl)]
    public extern static void BindProperty(IntPtr source, string sourceProperty, IntPtr target, string targetProperty, BindingFlags bindingFlags);

    [DllImport(Globals.LibGtk, EntryPoint="g_type_from_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static GType TypeFromName(string objectName);

    [DllImport(Globals.LibGtk, EntryPoint="g_type_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr TypeName(GType type);
    

    [DllImport(Globals.LibGtk, EntryPoint="g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBool(IntPtr obj, string name, bool value, IntPtr end);
    [DllImport(Globals.LibGtk, EntryPoint="g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetBool(IntPtr obj, string name, out bool value, IntPtr end);

    [DllImport(Globals.LibGtk, EntryPoint="g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetInt(IntPtr obj, string name, int value, IntPtr end);

    [DllImport(Globals.LibGtk, EntryPoint="g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetInt(IntPtr obj, string name, out int value, IntPtr end);

    [DllImport(Globals.LibGtk, EntryPoint="g_object_new", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr New(long type, IntPtr zero);

    [DllImport(Globals.LibGtk, EntryPoint="g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddWeakRef(IntPtr obj, FinalizerDelegate finalizer, IntPtr zero);
}


